using UnityEngine;
using System.Collections;
using Confactory;
using System;
using System.Collections.Generic;

public class ConActivePlayers : IConfactory
{
	public delegate void PlayerHandler(Player player);
	public event PlayerHandler PlayerRegisteredEvent;
	public event PlayerHandler PlayerUnRegisteredEvent;

	public ColorHandler ColorHandler { get; private set; }

	private List<Player> allPlayers = new List<Player>();

	public void ConStruct()
	{
		ColorHandler = new ColorHandler();
	}

	public void RegisterPlayer(Player player)
	{
		if (!allPlayers.Contains(player))
		{
			allPlayers.Add(player);
			if(PlayerRegisteredEvent != null)
			{
				PlayerRegisteredEvent(player);
            }
		}
		else
		{
			Debug.LogWarning("Cannot Register player: " + player + " while already registered");
		}
	}
	public void UnRegisterPlayer(Player player)
	{
		if (allPlayers.Contains(player))
		{
			allPlayers.Remove(player);
			ColorHandler.StopUsingColor(player.PlayerColor);
			if (PlayerUnRegisteredEvent != null)
			{
				PlayerUnRegisteredEvent(player);	
            }
		}
		else
		{
			Debug.LogWarning("Player: " + player + "Cannot be unregistered because it has not been registered");
		}
	}

	public Character CreateCharacterForPlayer(Player player)
	{
		Character character = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Player")).GetComponent<Character>();
		if(player.PlayerCharacter != null)
		{
			GameObject.Destroy(player.PlayerCharacter);
		}
		player.SetCharacter(character);
		return character;
	}

	public Player[] GetAllPlayers()
	{
		return allPlayers.ToArray();
	}

	public void ConClear()
	{

	}

	public void OnSceneSwitch(int newSceneIndex)
	{

	}
}
