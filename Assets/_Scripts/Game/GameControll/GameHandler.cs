using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using Ramses.Entities;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

	public delegate void BaseGameRulesHandler(BaseGameRules gameRules);
	public delegate void PlayerHandler(Player player);
	public delegate void CorpseHandler(Corpse corpse);

	public PlayerHandler PlayerCharacterSpawnedEvent;
	public CorpseHandler CorpseSpawnedEvent; // Game mods can let corpses disapear or even make zombies out of them if they so desire. IDEA (Haunted game mod maybe? be chaces by the ones you killed)
	public BaseGameRulesHandler GameRulesActivatedEvent;

	public ConActivePlayers ActivePlayers { get; private set; }
	public GameBattleHistoryLog BattleHistoryLog { get; private set; }
	public BaseGameRules ActiveGameRules { get; private set; } // Set on which game mod has been selected (In confactory)
	public GameObject[] Spawnpoints { get; private set; }
	public MassEntity SpawnArea;

	void Awake()
	{
		BattleHistoryLog = ConfactoryFinder.Instance.Give<ConGameBattleHistoryLog>(); // = new GameBattleHistoryLog();
		((ConGameBattleHistoryLog)BattleHistoryLog).Reset();
		
		ActiveGameRules = new TimeGameRules(this, 1);// For debugging! TODO Remove this and replace with a real selected mod
		ConfactoryFinder.Instance.Give<ConSelectedGameRules>().SetSelectedGameRules(ActiveGameRules);
		ActivePlayers = ConfactoryFinder.Instance.Give<ConActivePlayers>();
    }

	void Start()
	{
		Spawnpoints = ConfactoryFinder.Instance.Give<ConTags>().GetTagObjects(ConTags.TagList.Spawnpoint);
		SpawnArea = ConfactoryFinder.Instance.Give<ConEntityDatabase>().GetAnyEntity<MassEntity>("CamBoundsItem");
		StartGameRules();
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}

	private void StartGameRules()
	{
		ActiveGameRules.Start();
		if (GameRulesActivatedEvent != null)
		{
			GameRulesActivatedEvent(ActiveGameRules);
		}
	}

	// Spawning Game  
	public void SpawnAllPlayers()
	{
		int i = 0;
		foreach(Player p in ActivePlayers.GetAllPlayers())
		{
			SpawnPlayerCharacter(p, Spawnpoints[i]);
			i++;
		}
	}

	public void SpawnPlayerCharacter(Player player, GameObject spawnpoint, float timeToWaitTillSpawn)
	{
		StartCoroutine(WaitForSpawnPlayer(player, spawnpoint, timeToWaitTillSpawn));
	}

	public void SpawnWeapon(WeaponFactory.AllWeapons weapon, float timeToWaitTillSpawn)
	{
		StartCoroutine(WaitForSpawnWeapon(weapon, timeToWaitTillSpawn));
	}

	private IEnumerator WaitForSpawnPlayer(Player player, GameObject spawnpoint, float timeToWaitTillSpawn)
	{
		yield return new WaitForSeconds(timeToWaitTillSpawn);
		SpawnPlayerCharacter(player, spawnpoint);
	}

	private IEnumerator WaitForSpawnWeapon(WeaponFactory.AllWeapons weapon, float timeToWaitTillSpawn)
	{
		yield return new WaitForSeconds(timeToWaitTillSpawn);
		SpawnWeapon(weapon);
	}

	public void SpawnPlayerCharacter(Player player, GameObject spawnpoint)
	{
		if(player.PlayerCharacter != null)
		{
			RemoveEventListenersFromCharacter(player.PlayerCharacter);
			Destroy(player.PlayerCharacter.gameObject);
		}

		Character c = ActivePlayers.CreateCharacterForPlayer(player);
		c.gameObject.transform.position = spawnpoint.transform.position;
		CharacterSpawnObject spawnObject = Instantiate<GameObject>(Resources.Load<GameObject>("CharacterSpawnerObject")).GetComponent<CharacterSpawnObject>();
		spawnObject.transform.position = c.transform.position;
		spawnObject.Spawn(c);

		AddEventListenersToCharacter(c);

        if (PlayerCharacterSpawnedEvent != null)
		{
			PlayerCharacterSpawnedEvent(player);
        }
	}

	public void SpawnWeapon(WeaponFactory.AllWeapons weapon)
	{
		//Vector3 spawn = new Vector2(Random.Range(0, Screen.width), Screen.height);
		float spawnX = Random.Range(0, 100) * 0.01f;
		Vector3 spawn = new Vector3((SpawnArea.Size.x * spawnX) - SpawnArea.Size.x / 2, SpawnArea.Size.y / 2, -1);
        Weapon weaponSpawning = Instantiate<Weapon>(WeaponFactory.GetWeaponObject(weapon));
		weaponSpawning.transform.eulerAngles = new Vector3(0, 0, -92);
		SpawnObject(weaponSpawning, spawn, new Vector2(0, -0.6f));
	}

	public void SpawnObject(MonoEntity objectEntity, Vector2 position, Vector2? offsetObject = null)
	{
		objectEntity.gameObject.transform.position = new Vector3(position.x,position.y,-1);
		EntitySpawnObject spawnAnimationObject = Instantiate<EntitySpawnObject>(Resources.Load<EntitySpawnObject>("SpawnerObject"));
		spawnAnimationObject.transform.position = objectEntity.transform.position;
		if (offsetObject.HasValue)
		{
			objectEntity.gameObject.transform.position = objectEntity.gameObject.transform.position + new Vector3(offsetObject.Value.x, offsetObject.Value.y, 0);
		}
        spawnAnimationObject.Spawn(objectEntity);
    }
	// End spawning section game

	// Game Controll
	public void EndGame()
	{
		//Debug.Log("Global End Game Method to end the game and its gamemod mechanics");
		SceneManager.LoadScene("End");
	}


	// Events
	private void AddEventListenersToCharacter(Character character)
	{
		character.CharacterGotKilledEvent += OnCharacterGotKilledEvent;
		character.CharacterDestroyEvent += OnCharacterDestroyEvent;
	}

	private void RemoveEventListenersFromCharacter(Character character)
	{
		character.CharacterGotKilledEvent -= OnCharacterGotKilledEvent;
		character.CharacterDestroyEvent -= OnCharacterDestroyEvent;
	}

	private void OnCharacterGotKilledEvent(Character killed, Character killer)
	{
		// place in history log
		Player killerP = ActivePlayers.FindPlayerOfActiveCharacter(killer);
		Player killedP = ActivePlayers.FindPlayerOfActiveCharacter(killed);

		BattleHistoryLog.AddData(killerP, killedP);
    }

	private void OnCharacterDestroyEvent(Character character)
	{
		if (!character.IsAlive)
		{
			Player playerOfCharacter = ActivePlayers.FindPlayerOfActiveCharacter(character);
			Player killer = BattleHistoryLog.GetLastKillerOfPlayer(playerOfCharacter);

			Corpse corpse = Instantiate<Corpse>(Resources.Load<Corpse>("PlayerCorpse"));
			Rigidbody2D corpseRigid = corpse.GetComponent<Rigidbody2D>();

			corpseRigid.gravityScale = character.CharacterRigidbody2D.gravityScale;
			corpseRigid.velocity = character.CharacterRigidbody2D.velocity;

            corpse.transform.position = character.transform.position;
			corpse.transform.localScale = character.transform.localScale;

			Color color = corpse.SpriteRenderer.color;
			Color colorP = playerOfCharacter.PlayerRealColor; //ColorHandler.ColorsToColor(playerOfCharacter.PlayerColor);
			float playerColorIntensity = 0.2f;
			colorP *= playerColorIntensity;
			color += colorP;
			corpse.SpriteRenderer.color = color;

			corpse.SetCorpseInfo(playerOfCharacter, killer);

			RemoveEventListenersFromCharacter(character);

			Destroy(character.gameObject);

			if(CorpseSpawnedEvent != null)
			{
				CorpseSpawnedEvent(corpse); //Other game mods can interact with the corpse spawn. Even delete them and spawn zombie corpses or something crasy
			}
		}
	}
}
