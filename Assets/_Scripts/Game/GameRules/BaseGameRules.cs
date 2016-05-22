using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseGameRules
{
	public GameHandler GameHandler { get; private set; }
	protected ConAudioManager audioManager { get; private set; }

	public BaseGameRules()
	{
		audioManager = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConAudioManager>();
    }

	public virtual void Start(GameHandler handler)
	{
		Stop();
		this.GameHandler = handler;
		GameHandler.PlayerCharacterSpawnedEvent += OnPlayerCharacterSpawn;
		GameHandler.CorpseSpawnedEvent += OnCorpseSpawnedEvent;
		Debug.Log("Game Started");
	}

	public virtual void Stop()
	{
		if (GameHandler != null)
		{
			GameHandler.PlayerCharacterSpawnedEvent -= OnPlayerCharacterSpawn;
			GameHandler.CorpseSpawnedEvent -= OnCorpseSpawnedEvent;
		}
		Debug.Log("Game Ended");
	}

	public virtual void Clear()
	{
		GameHandler = null;
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
