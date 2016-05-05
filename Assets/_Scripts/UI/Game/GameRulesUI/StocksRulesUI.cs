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

		foreach (UIPlayerInfo playerInfo in uiGameRules.PlayerInfos.GetUIPlayerInfos(true))
		{
			stocksGameRules = (StockGameRules)uiGameRules.GameHandler.ActiveGameRules;
			playerInfo.SetScoreIndication(UIPlayerInfo.ScoreIndications.Stocks);
			playerInfo.CurrentIndication.IndicationText.text = stocksGameRules.GetStockAmountOfPlayer(playerInfo.LinkedPlayer).ToString();
		}
		stocksGameRules.PlayerStockChangedEvent -= OnPlayerStockChangedEvent;
		stocksGameRules.PlayerStockChangedEvent += OnPlayerStockChangedEvent;
	}

	public void Stop()
	{
		stocksGameRules.PlayerStockChangedEvent -= OnPlayerStockChangedEvent;
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
}
