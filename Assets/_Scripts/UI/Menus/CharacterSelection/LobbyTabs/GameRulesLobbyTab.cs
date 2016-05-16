using UnityEngine;
using System.Collections;
using Ramses.SectionButtons;
using UnityEngine.UI;
using Ramses.Confactory;

public class GameRulesLobbyTab : LobbyTab
{
	[SerializeField] private SectionStringListButton gameRulesButton;
	[SerializeField] private Text gameRulesText;

	[SerializeField] private SectionIntListButton stocksButton;
	[SerializeField] private Text stocksText;

	[SerializeField] private SectionIntListButton playtimeButton;
	[SerializeField] private Text playtimeText;

	private ConSelectedGameRules conSelectedGameRules;

	private void Start()
	{
		conSelectedGameRules = ConfactoryFinder.Instance.Give<ConSelectedGameRules>();
		SetListenersActiveState(true);
		BaseGameRules selectedRules = conSelectedGameRules.GetSelectedGameRules();
        if (selectedRules == null)
		{
			OnGameRulesButtonChangedEvent(null, null, gameRulesButton.CurrentValue);
			OnStocksButtonChangedEvent(null, 0, stocksButton.CurrentValue);
			OnPlaytimeButtonChangedEvent(null, 0, playtimeButton.CurrentValue);
		}
		else
		{
			if(selectedRules is StockGameRules)
			{
				gameRulesButton.JumpToIndex(0);
                stocksButton.JumpToIndex(((StockGameRules)selectedRules).StartingStockAmount);
			}
			if(selectedRules is TimeGameRules)
			{
				gameRulesButton.JumpToIndex(1);
				playtimeButton.JumpToIndex(((TimeGameRules)selectedRules).StartingTimeInMinutes - 1);
			}
		}
	}

	private void SetListenersActiveState(bool state)
	{
		gameRulesButton.ValueChangedEvent -= OnGameRulesButtonChangedEvent;
		stocksButton.ValueChangedEvent -= OnStocksButtonChangedEvent;
		playtimeButton.ValueChangedEvent -= OnPlaytimeButtonChangedEvent;

		if (state)
		{
			gameRulesButton.ValueChangedEvent += OnGameRulesButtonChangedEvent;
			stocksButton.ValueChangedEvent += OnStocksButtonChangedEvent;
			playtimeButton.ValueChangedEvent += OnPlaytimeButtonChangedEvent;
		}
	}

	private void OnGameRulesButtonChangedEvent(SectionListButton<string> button, string oldValue, string newValue)
	{
		gameRulesText.text = newValue;
		SetOptions(gameRulesButton.CurrentValue);
	}

	private void OnStocksButtonChangedEvent(SectionListButton<int> button, int oldValue, int newValue)
	{
		stocksText.text = "Stocks: " + newValue;
	}

	private void OnPlaytimeButtonChangedEvent(SectionListButton<int> button, int oldValue, int newValue)
	{
		playtimeText.text = newValue + " Min";
	}

	public override void Close()
	{
		base.Close();
		// Save data
		BaseGameRules rulesToSave = null;
		switch(gameRulesButton.CurrentValue)
		{
			case "Stocks":
				rulesToSave = new StockGameRules(stocksButton.CurrentValue);
                break;
			case "Time":
				rulesToSave = new TimeGameRules(playtimeButton.CurrentValue, stocksButton.CurrentValue);
				break;
        }
		conSelectedGameRules.SetSelectedGameRules(rulesToSave);
	}

	private void SetOptions(string rules)
	{
		if(rules == "Stocks")
		{
			stocksButton.LockItem(0);
			playtimeButton.SetActiveState(false);
        }
		else if(rules == "Time")
		{
			stocksButton.UnLockItem(0);
			playtimeButton.SetActiveState(true);
		}
	}

	public override void SetActiveState(bool value)
	{
		base.SetActiveState(value);
		SetOptions(gameRulesButton.CurrentValue);
	}
}
