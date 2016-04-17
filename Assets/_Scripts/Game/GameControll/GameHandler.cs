using UnityEngine;
using System.Collections;
using Confactory;
public class GameHandler : MonoBehaviour {

	public delegate void PlayerHandler(Player player);
	public delegate void CorpseHandler(Corpse corpse);

	public PlayerHandler PlayerCharacterSpawnedEvent;
	public CorpseHandler CorpseSpawnedEvent; // Game mods can let corpses disapear or even make zombies out of them if they so desire. IDEA (Haunted game mod maybe? be chaces by the ones you killed)

	private BaseGameRules activeGameRules;
	private GameBattleHistoryLog battleHistoryLog = new GameBattleHistoryLog();
	private ConActivePlayers activePlayers;
	private GameObject[] spawnpoints;

	void Awake()
	{
		activePlayers = ConfactoryFinder.Instance.Give<ConActivePlayers>();
    }

	void Start()
	{
		spawnpoints = ConfactoryFinder.Instance.Give<ConTags>().GetTagObjects(ConTags.TagList.Spawnpoint);
		SpawnAllPlayers();
	}

	public void SpawnAllPlayers()
	{
		int i = 0;
		foreach(Player p in activePlayers.GetAllPlayers())
		{
			SpawnPlayer(p, spawnpoints[i]);
			i++;
		}
	}

	public void SpawnPlayer(Player player, GameObject spawnpoint)
	{
		// if active character is null then create character
		// if the character is dead then create a new one and delete old one or inactive it so it stays on the ground (for maybe x amount of time)
		// if there is a character then just teleport him to the spawn and play the spawn animation (dont make a new one so it keeps its weapons and everything)

		if(player.PlayerCharacter != null)
		{
			RemoveEventListenersFromCharacter(player.PlayerCharacter);
		}

		Character c = activePlayers.CreateCharacterForPlayer(player);
		c.gameObject.transform.position = spawnpoint.transform.position;

		AddEventListenersToCharacter(c);

        if (PlayerCharacterSpawnedEvent != null)
		{
			PlayerCharacterSpawnedEvent(player);
        }
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
		Player killerP = activePlayers.FindPlayerOfActiveCharacter(killer);
		Player killedP = activePlayers.FindPlayerOfActiveCharacter(killed);

		battleHistoryLog.AddData(killerP, killedP);
    }

	private void OnCharacterDestroyEvent(Character character)
	{
		// Delete character to place special item. look into battlehistory to fill in corpse info
		if (!character.IsAlive)
		{
			Player playerOfCharacter = activePlayers.FindPlayerOfActiveCharacter(character);
			Player killer = battleHistoryLog.GetLastKillerOfPlayer(playerOfCharacter);

			Corpse corpse = Instantiate<Corpse>(Resources.Load<Corpse>("PlayerCorpse"));
			corpse.transform.position = character.transform.position;

			corpse.playerOwnedCorpse = playerOfCharacter;
			corpse.killerOfCorpse = killer;

			RemoveEventListenersFromCharacter(character);

			Destroy(character.gameObject);

			if(CorpseSpawnedEvent != null)
			{
				CorpseSpawnedEvent(corpse);
			}
		}
	}
}
