using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBattleHistoryLog
{
	protected Dictionary<Player, List<Player>> playerKillsData = new Dictionary<Player, List<Player>>();
	protected Dictionary<Player, List<Player>> playerDeathsData = new Dictionary<Player, List<Player>>();

	public void AddData(Player killer, Player killed)
	{
		if(!playerKillsData.ContainsKey(killer))
		{
			playerKillsData.Add(killer, new List<Player>());
		}

		if(!playerDeathsData.ContainsKey(killed))
		{
			playerDeathsData.Add(killed, new List<Player>());
        }

		playerKillsData[killer].Add(killed); // if killer == null then its a world kill
		playerDeathsData[killed].Add(killer); 
    }

	public Player GetFirstKilledOfPlayer(Player player)
	{
		if (playerKillsData.ContainsKey(player))
		{
			Player[] allkills = playerKillsData[player].ToArray();
			return allkills[0];
		}
		return null;
	}

	public Player GetFirstKillerOfPlayer(Player player)
	{
		if (playerDeathsData.ContainsKey(player))
		{
			Player[] allkillers = playerDeathsData[player].ToArray();
			return allkillers[0];
		}
		return null;
	}

	public Player GetLastKillerOfPlayer(Player player)
	{
		if (playerDeathsData.ContainsKey(player))
		{
			Player[] allkillers = playerDeathsData[player].ToArray();
			return allkillers[allkillers.Length - 1];
		}
		return null;
    }

	public Player GetLastKilledOfPlayer(Player player)
	{
		if (playerKillsData.ContainsKey(player))
		{
			Player[] allkills = playerKillsData[player].ToArray();
			return allkills[allkills.Length - 1];
		}
		return null;
	}

	public Player[] GetAllKillsOfPlayer(Player player)
	{
		if (playerKillsData.ContainsKey(player))
		{
			return playerKillsData[player].ToArray();
		}
		return new Player[] { };
	}

	public Player[] GetAllKillersOfPlayer(Player player)
	{
		if (playerDeathsData.ContainsKey(player))
		{
			return playerDeathsData[player].ToArray();
		}
		return new Player[] { };
	}

}
