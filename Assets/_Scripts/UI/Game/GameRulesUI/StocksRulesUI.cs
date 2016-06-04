using UnityEngine;
using System.Collections;
using System;

public class StocksRulesUI : IBaseGameRulesUI
{
	private UIGameRules uiGameRules;
	StockGameRules stocksGameRules;

	private UIPlayerInfo.ScoreIndications currentIndicationType = UIPlayerInfo.ScoreIndications.None;

	public void Start(UIGameRules uiGameRules)
	{
		this.uiGameRules = uiGameRules;
		stocksGameRules = (StockGameRules)uiGameRules.GameHandler.ActiveGameRules;
		SetCurrentIndicationType(stocksGameRules.StartingStockAmount > 0 ? UIPlayerInfo.ScoreIndications.Stocks : UIPlayerInfo.ScoreIndications.Score);
	}

	public void Stop()
	{
		CleanListenersFromRules(stocksGameRules);
		stocksGameRules.GameHandler.BattleHistoryLog.DataAddedEvent -= OnBattleDataAddedEvent;
		uiGameRules.Clock.gameObject.SetActive(false);
		ClockStopped(0);
    }

	private void ClockStopped(int tiks)
	{
		if (stocksGameRules is TimeGameRules)
		{
			((TimeGameRules)stocksGameRules).Timer.TimerStoppedEvent -= ClockStopped;
			((TimeGameRules)stocksGameRules).Timer.TimerTikkedEvent -= OnTimerTik;
		}
	}

	private void OnBattleDataAddedEvent(GameBattleHistoryLog log, Player killer, Player killed)
	{
		uiGameRules.PlayerInfos.GetUIPlayerInfo(killer).CurrentIndication.IndicationText.text = "Kills: " + log.GetAllKillsOfPlayer(killer).Length;
    }

	private void OnPlayerStockChangedEvent(Player player, int stocks)
	{
		UIPlayerInfo playerInfo = uiGameRules.PlayerInfos.GetUIPlayerInfo(player);
		playerInfo.CurrentIndication.IndicationText.text = stocks.ToString();
		if (currentIndicationType != UIPlayerInfo.ScoreIndications.Stocks)
		{ 
			SetCurrentIndicationType(UIPlayerInfo.ScoreIndications.Stocks);
		}
        if (stocks == 0)
		{
			playerInfo.BackgroundImage.color = Color.gray;
			playerInfo.BackgroundImage.SetAlpha(0.8f);
			playerInfo.PortraitImage.SetAlpha(0.8f);
		}
	}

	private void SetCurrentIndicationType(UIPlayerInfo.ScoreIndications type)
	{
		currentIndicationType = type;
		CleanListenersFromRules(stocksGameRules);
		AddListenersToRules(stocksGameRules);
		uiGameRules.Clock.gameObject.SetActive(false);
		foreach (UIPlayerInfo info in uiGameRules.PlayerInfos.GetUIPlayerInfos(true))
		{
			info.SetScoreIndication(currentIndicationType);
			if (currentIndicationType == UIPlayerInfo.ScoreIndications.Stocks)
			{
				info.CurrentIndication.IndicationText.text = stocksGameRules.GetStockAmountOfPlayer(info.LinkedPlayer).ToString();
			}
			else
			{
				info.CurrentIndication.IndicationText.text = "Kills: 0";
			}
		}
		if (!stocksGameRules.SuddenDeathActivated)
		{
			if (stocksGameRules is TimeGameRules)
			{
				TimeGameRules timeRules = (TimeGameRules)stocksGameRules;
				uiGameRules.Clock.gameObject.SetActive(true);
				uiGameRules.Clock.IndicationText.text = TimerUtils.MinutesToClockString(timeRules.StartingTimeInMinutes);
			}
		}
	}

	private void AddListenersToRules(BaseGameRules rules)
	{
		CleanListenersFromRules(rules);
		if (currentIndicationType == UIPlayerInfo.ScoreIndications.Score)
		{
			stocksGameRules.GameHandler.BattleHistoryLog.DataAddedEvent += OnBattleDataAddedEvent;
		}
		stocksGameRules.PlayerStockChangedEvent += OnPlayerStockChangedEvent;
		if (stocksGameRules is TimeGameRules)
		{
			TimeGameRules timeRules = (TimeGameRules)stocksGameRules;
			timeRules.Timer.TimerTikkedEvent += OnTimerTik;
			timeRules.Timer.TimerStoppedEvent += ClockStopped;
		}
	}

	private void CleanListenersFromRules(BaseGameRules rules)
	{
		stocksGameRules.GameHandler.BattleHistoryLog.DataAddedEvent -= OnBattleDataAddedEvent;
		stocksGameRules.PlayerStockChangedEvent -= OnPlayerStockChangedEvent;
		if (stocksGameRules is TimeGameRules)
		{
			TimeGameRules timeRules = (TimeGameRules)stocksGameRules;
			timeRules.Timer.TimerTikkedEvent -= OnTimerTik;
			timeRules.Timer.TimerStoppedEvent -= ClockStopped;
		}
	}

	private void OnTimerTik(int currentTik)
	{
		int secondsRemaining = ((TimeGameRules)stocksGameRules).Timer.TimesToLoop - currentTik;
        uiGameRules.Clock.IndicationText.text = TimerUtils.SecondsToClockString(secondsRemaining);
    }
}
