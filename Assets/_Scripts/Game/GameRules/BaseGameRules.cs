using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseGameRules
{
	protected GameHandler gameHandler { get; private set; }

	public BaseGameRules(GameHandler handler)
	{
		this.gameHandler = handler;
		gameHandler.PlayerCharacterSpawnedEvent += OnPlayerCharacterSpawn;
        gameHandler.CorpseSpawnedEvent += OnCorpseSpawnedEvent;
    }

	public virtual void Start()
	{
		Debug.Log("Game Started");
	}

	public virtual void OnPlayerCharacterSpawn(Player player)
	{

	}

	public virtual void OnCorpseSpawnedEvent(Corpse corpse)
	{

	}

	/// <summary>
	/// Returns an array of players sorted on rank.
	/// Best player key with int 0 is first rank. The higher the number, the lower the rank.
	/// </summary>
	/// <returns></returns>
	public virtual Dictionary<Player, int> GetPlayersSortedOnRank()
	{
		Debug.LogError("Unimplemented Method 'GetPlayersSortedOnRank' is called but not overwriten");
		return null;
	}

	public int GetPlayerRank(Player player)
	{
		return GetPlayersSortedOnRank()[player];
    }
}
