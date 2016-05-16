using UnityEngine;
using System.Collections;

public class TimeGameRules : StockGameRules {

	public readonly int StartingTimeInMinutes;

	public Timer Timer { get; private set; }

	public TimeGameRules(int timeInMinutes, int stocks = 0) : base(stocks)
	{
		StartingTimeInMinutes = timeInMinutes;

		Timer = new Timer(1, StartingTimeInMinutes * 60);
		Timer.TimerTikkedEvent += OnTimerTikkedEvent;
    }

	public override void Start(GameHandler handler)
	{
		base.Start(handler);
		Timer.Start();
	}

	private void OnTimerTikkedEvent(int tik)
	{
		int secondsLeft = Timer.TimesToLoop - tik;
		switch (secondsLeft)
		{
			case 3:
				audioManager.PlayAudio("VoiceThree");
				break;
			case 2:
				audioManager.PlayAudio("VoiceTwo");
				break;
			case 1:
				audioManager.PlayAudio("VoiceOne");
				break;
			case 0:
				EndGame();
				break;
		}
		
    }

	protected override void EndGame()
	{
		Timer.Stop();
        base.EndGame();
	}
}
