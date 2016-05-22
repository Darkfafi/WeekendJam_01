using UnityEngine;
using System.Collections;

public class UIGameRules : MonoBehaviour {

	public GameHandler GameHandler { get { return gameHandler; } }
	public UIPlayerInfos PlayerInfos { get { return playerInfos; } }
	public TextIndicationUI Clock { get { return clock; } }
	public ConAudioManager audioManager { get; private set; }

	[SerializeField] private GameHandler gameHandler;
	[SerializeField] private UIPlayerInfos playerInfos;
	[SerializeField] private TextIndicationUI clock;

	private IBaseGameRulesUI currentBaseRulesUI = null;

	void Awake()
	{
		audioManager = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConAudioManager>();
		GameHandler.GameRulesActivatedEvent += OnGameRulesActivatedEvent;
		GameHandler.GameRulesDeactivatedEvent += OnGameRulesDeactivatedEvent;
	}

	void OnDestroy()
	{
		OnGameRulesDeactivatedEvent(gameHandler.ActiveGameRules);
    }

	protected virtual void OnGameRulesActivatedEvent(BaseGameRules rulesActivated)
	{
		if (currentBaseRulesUI != null)
		{
			currentBaseRulesUI.Stop();
			currentBaseRulesUI = null;
		}

		if (rulesActivated is StockGameRules)
		{
			currentBaseRulesUI = new StocksRulesUI();
		}

		currentBaseRulesUI.Start(this);
	}

	protected virtual void OnGameRulesDeactivatedEvent(BaseGameRules rulesActivated)
	{
		GameHandler.GameRulesActivatedEvent -= OnGameRulesActivatedEvent;
		GameHandler.GameRulesDeactivatedEvent -= OnGameRulesDeactivatedEvent;
		if (currentBaseRulesUI != null)
		{
			currentBaseRulesUI.Stop();
		}
	}
}
