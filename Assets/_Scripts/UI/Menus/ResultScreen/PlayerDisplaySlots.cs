using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Confactory;

public class PlayerDisplaySlots : MonoBehaviour {

	[SerializeField] PlayerDisplay[] playerDisplays;

	private ConActivePlayers conActivePlayers;
	private ConSelectedGameRules conSelectedGameRules;

	private void Awake()
	{
		conActivePlayers = ConfactoryFinder.Instance.Give<ConActivePlayers>();
		conSelectedGameRules = ConfactoryFinder.Instance.Give<ConSelectedGameRules>();

		SetPlayerDisplays();
    }

	private void SetPlayerDisplays()
	{
		Player[] players = conActivePlayers.GetAllPlayers();
		foreach (Player p in players)
		{
			playerDisplays[p.playerId].SetPlayerOfSlot(p);
			playerDisplays[p.playerId].SetRankOfSlot(conSelectedGameRules.GetSelectedGameRules().GetPlayerRank(p));
			SetRulesSpecificsOnDisplay(playerDisplays[p.playerId]);
        }
	}

	private void SetRulesSpecificsOnDisplay(PlayerDisplay display)
	{
		BaseGameRules rules = conSelectedGameRules.GetSelectedGameRules();
		
		if(rules is StockGameRules)
		{
			display.StocksEndResultDisplay.DisplayPlayerResult(display.Player);
		}
	}
}
