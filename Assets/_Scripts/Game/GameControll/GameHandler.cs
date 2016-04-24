using UnityEngine;
using System.Collections;
using Ramses.Confactory;

public class GameHandler : MonoBehaviour {

	public delegate void PlayerHandler(Player player);
	public delegate void CorpseHandler(Corpse corpse);

	public PlayerHandler PlayerCharacterSpawnedEvent;
	public CorpseHandler CorpseSpawnedEvent; // Game mods can let corpses disapear or even make zombies out of them if they so desire. IDEA (Haunted game mod maybe? be chaces by the ones you killed)

	public ConActivePlayers ActivePlayers { get; private set; }
	public GameBattleHistoryLog BattleHistoryLog { get; private set; }
	public BaseGameRules ActiveGameRules { get; private set; } // Set on which game mod has been selected (In confactory)
	public GameObject[] Spawnpoints { get; private set; }

	void Awake()
	{ 
		BattleHistoryLog = new GameBattleHistoryLog();
		ActiveGameRules = new StockGameRules(this, 3);// For debugging! TODO Remove this and replace with a real selected mod
		ActivePlayers = ConfactoryFinder.Instance.Give<ConActivePlayers>();

		ActiveGameRules.Start();
	}

	void Start()
	{
		Spawnpoints = ConfactoryFinder.Instance.Give<ConTags>().GetTagObjects(ConTags.TagList.Spawnpoint);
		SpawnAllPlayers();
	}

	public void SpawnAllPlayers()
	{
		int i = 0;
		foreach(Player p in ActivePlayers.GetAllPlayers())
		{
			SpawnPlayerCharacter(p, Spawnpoints[i]);
			i++;
		}
	}

	//TODO Spawn animation  
	public void SpawnPlayerCharacter(Player player, GameObject spawnpoint)
	{
		if(player.PlayerCharacter != null)
		{
			RemoveEventListenersFromCharacter(player.PlayerCharacter);
			Destroy(player.PlayerCharacter.gameObject);
		}

		Character c = ActivePlayers.CreateCharacterForPlayer(player);
		c.gameObject.transform.position = spawnpoint.transform.position;

		AddEventListenersToCharacter(c);

        if (PlayerCharacterSpawnedEvent != null)
		{
			PlayerCharacterSpawnedEvent(player);
        }
	}

	public void EndGame()
	{
		Debug.Log("Global End Game Method to end the game and its gamemod mechanics");
	}

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
			Color colorP = ColorHandler.ColorsToColor(playerOfCharacter.PlayerColor);
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
