using UnityEngine;
using System.Collections;
using Confactory;
using System;
using System.Collections.Generic;

public class ConActivePlayers : IConfactory {
	 
	private List<PlayerInfo> allPlayers = new List<PlayerInfo>();

	public void ConStruct()
	{

	}

	public void RegisterPlayer(PlayerInfo player)
	{
		if (!allPlayers.Contains(player))
		{
			allPlayers.Add(player);
		}
		else
		{
			Debug.LogWarning("Cannot Register player: " + player + " while already registered");
		}
	}
	public void UnRegisterPlayer(PlayerInfo player)
	{
		if (allPlayers.Contains(player))
		{
			allPlayers.Remove(player);
		}
		else
		{
			Debug.LogWarning("Player: " + player + "Cannot be unregistered because it has not been registered");
		}
	}
	public PlayerInfo[] GetAllPlayers()
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
