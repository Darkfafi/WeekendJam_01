using UnityEngine;
using System.Collections;
using System;

public class StocksRulesUI : IBaseGameRulesUI
{
	private UIGameRules uiGameRules;
	StockGameRules stocksGameRules;

	public void Start(UIGameRules uiGameRules)
	{
		this.uiGameRules = uiGameRules;
		stocksGameRules = (StockGameRules)uiGameRules.GameHandler.ActiveGameRules;
		foreach (UIPlayerInfo playerInfo in uiGameRules.PlayerInfos.GetUIPlayerInfos(true))
		{
			if (stocksGameRules.StartingStockAmount > 0)
			{
				playerInfo.SetScoreIndication(UIPlayerInfo.ScoreIndications.Stocks);
				playerInfo.CurrentIndication.IndicationText.text = stocksGameRules.GetStockAmountOfPlayer(playerInfo.LinkedPlayer).ToString();
				stocksGameRules.PlayerStockChangedEvent -= OnPlayerStockChangedEvent;
				stocksGameRules.PlayerStockChangedEvent += OnPlayerStockChangedEvent;
			}
			else
			{
				playerInfo.SetScoreIndication(UIPlayerInfo.ScoreIndications.Score);
				stocksGameRules.gameHandler.BattleHistoryLog.DataAddedEvent -= OnBattleDataAddedEvent;
				stocksGameRules.gameHandler.BattleHistoryLog.DataAddedEvent += OnBattleDataAddedEvent;
				playerInfo.CurrentIndication.IndicationText.text = "Kills: 0";
            }

			if(stocksGameRules is TimeGameRules)
			{
				TimeGameRules timeRules = (TimeGameRules)stocksGameRules;
				uiGameRules.Clock.gameObject.SetActive(true);
				uiGameRules.Clock.IndicationText.text = TimerUtils.MinutesToClockString(timeRules.StartingTimeInMinutes);
				timeRules.Timer.TimerTikkedEvent -= OnTimerTik;
				timeRules.Timer.TimerTikkedEvent += OnTimerTik;
            }
		}
		
	}

	public void Stop()
	{
		stocksGameRules.PlayerStockChangedEvent -= OnPlayerStockChangedEvent;
		stocksGameRules.gameHandler.BattleHistoryLog.DataAddedEvent -= OnBattleDataAddedEvent;
		uiGameRules.Clock.gameObject.SetActive(false);
		if (stocksGameRules is TimeGameRules)
		{
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

		if (stocks == 0)
		{
			playerInfo.BackgroundImage.color = Color.gray;
			playerInfo.BackgroundImage.SetAlpha(0.8f);
			playerInfo.PortraitImage.SetAlpha(0.8f);
		}
	}

	private void OnTimerTik(int currentTik)
	{
		uiGameRules.Clock.IndicationText.text = TimerUtils.SecondsToClockString(((TimeGameRules)stocksGameRules).Timer.TimesToLoop - currentTik);
    }
}
