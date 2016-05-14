using UnityEngine;
using System.Collections;

public class TimeGameRules : StockGameRules {

	public readonly int StartingTimeInMinutes;

	public Timer Timer { get; private set; }

	public TimeGameRules(GameHandler handler, int timeInMinutes, int stocks = 0) : base(handler, stocks)
	{
		StartingTimeInMinutes = timeInMinutes;

		Timer = new Timer(1, StartingTimeInMinutes * 60);
		Timer.TimerEndedEvent += OnTimerEndedEvent;
    }

	public override void Start()
	{
		base.Start();
		Timer.Start();
	}

	private void OnTimerEndedEvent()
	{
		gameHandler.EndGame();
	}
}
