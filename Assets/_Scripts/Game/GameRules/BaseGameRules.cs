using UnityEngine;
using System.Collections;

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
}
