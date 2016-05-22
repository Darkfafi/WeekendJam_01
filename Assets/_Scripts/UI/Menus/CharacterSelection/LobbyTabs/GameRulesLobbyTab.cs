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
        
		OnGameRulesButtonChangedEvent(null, null, gameRulesButton.CurrentValue);
		OnStocksButtonChangedEvent(null, 0, stocksButton.CurrentValue);
		OnPlaytimeButtonChangedEvent(null, 0, playtimeButton.CurrentValue);

		if (selectedRules != null)
		{
			SetRulesButton(selectedRules);
            if (selectedRules is StockGameRules)
			{
				stocksButton.JumpToIndex(((StockGameRules)selectedRules).StartingStockAmount);
			}

			if (selectedRules is TimeGameRules)
			{
				playtimeButton.JumpToIndex(((TimeGameRules)selectedRules).StartingTimeInMinutes - 1);
			}
		}
	}

	private void SetRulesButton(BaseGameRules rules)
	{
		if (rules is StockGameRules)
		{
			gameRulesButton.JumpToIndex(0);
		}

		if (rules is TimeGameRules)
		{
			gameRulesButton.JumpToIndex(1);
		}
	}

	public override void GetInput(ConGameInputBindings.BindingTypes type, InputAction action)
	{
		base.GetInput(type, action);
		if((action.Type == InputItem.InputType.KeyCode && action.KeyActionValue == InputAction.KeyAction.OnKeyDown) ||
			action.Type == InputItem.InputType.Axis && (action.LastValue == 0 || action.LastValue == InputAction.NOT_IN_USE_VALUE))
		{
			if(action.Name == InputNames.DOWN)
			{
				NextButton();
			}
			else if(action.Name == InputNames.UP)
			{
				PreviousButton();
			}
			if(CurrentButton is ISectionListButton)
			{
				ISectionListButton button = (ISectionListButton)CurrentButton;
				if(action.Name == InputNames.LEFT)
				{
					button.UsePreviousButton();
				}
				else if(action.Name == InputNames.RIGHT)
				{
					button.UseNextButton();
				}
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
