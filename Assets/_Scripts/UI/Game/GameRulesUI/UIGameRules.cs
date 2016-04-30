using UnityEngine;
using System.Collections;

public class UIGameRules : MonoBehaviour {

	public GameHandler GameHandler { get { return gameHandler;  } }
    public UIPlayerInfos PlayerInfos { get { return playerInfos;  } }

	[SerializeField] private GameHandler gameHandler;
	[SerializeField] private UIPlayerInfos playerInfos;

	private IBaseGameRulesUI currentBaseRulesUI = null;

	void Start ()
	{
		if (gameHandler.ActiveGameRules is StockGameRules)
		{
			currentBaseRulesUI = new StocksRulesUI();
        }

		currentBaseRulesUI.Start(this);
	}

	void OnDestroy()
	{
		currentBaseRulesUI.Stop();
	}
}
