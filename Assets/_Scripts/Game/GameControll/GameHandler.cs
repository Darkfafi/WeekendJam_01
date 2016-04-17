using UnityEngine;
using System.Collections;
using Confactory;
public class GameHandler : MonoBehaviour {

	public delegate void PlayerHandler(Player player);
	public PlayerHandler PlayerCharacterSpawnedEvent;

	private BaseGameRules activeGameRules;

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
		Character c = activePlayers.CreateCharacterForPlayer(player);
		c.gameObject.transform.position = spawnpoint.transform.position;

		if(PlayerCharacterSpawnedEvent != null)
		{
			PlayerCharacterSpawnedEvent(player);
        }
	}

	
}
