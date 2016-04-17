using UnityEngine;
using System.Collections;

public class BaseGameRules
{
	private GameHandler gameHandler;

	public BaseGameRules(GameHandler handler)
	{
		this.gameHandler = handler;
	}

	public virtual void OnPlayerCharacterSpawn(Player player)
	{

	}

	public virtual void OnPlayerCharacterDeath(Player player)
	{

	}
}
