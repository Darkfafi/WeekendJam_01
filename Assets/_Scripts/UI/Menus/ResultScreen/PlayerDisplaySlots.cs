using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Confactory;
using UnityEngine.SceneManagement;

public class PlayerDisplaySlots : MonoBehaviour {

	[SerializeField] PlayerDisplay[] playerDisplays;

	private ConActivePlayers conActivePlayers;
	private ConSelectedGameRules conSelectedGameRules;
	private MultiInputUser inputUser;
    private void Awake()
	{
		conActivePlayers = ConfactoryFinder.Instance.Give<ConActivePlayers>();
		conSelectedGameRules = ConfactoryFinder.Instance.Give<ConSelectedGameRules>();
		ActivateKeyListener();
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

	private void ActivateKeyListener()
	{
		inputUser = gameObject.AddComponent<MultiInputUser>();
		Player[] players = conActivePlayers.GetAllPlayers();
		foreach (Player p in players)
		{
			inputUser.AddBindingType(p.BindingType);
        }
		inputUser.InputBindingUsedEvent += OnInputBindingUsedEvent;
    }

	private void OnInputBindingUsedEvent(ConGameInputBindings.BindingTypes type, InputAction action)
	{
		if(action.Name == InputNames.ATTACK)
		{
			SceneManager.LoadScene("Lobby");
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
