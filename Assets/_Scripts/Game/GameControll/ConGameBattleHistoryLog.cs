using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Ramses.Confactory;
using System;

public class ConGameBattleHistoryLog : GameBattleHistoryLog, IConfactory {

	public void Reset()
	{
		playerKillsData = new Dictionary<Player, List<Player>>();
		playerDeathsData = new Dictionary<Player, List<Player>>();
	}

	public void ConClear()
	{

	}

	public void ConStruct()
	{

	}

	public void OnSceneSwitch(int newSceneIndex)
	{

	}
}
