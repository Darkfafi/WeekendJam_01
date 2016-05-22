using UnityEngine;
using System.Collections;
using Ramses.Confactory;

public class Timer {

	public delegate void TimerHandler();
	public delegate void TimerTikHandler(int timesTikked);

	public event TimerHandler TimerStartedEvent;
	public event TimerHandler TimerPausedEvent;
	public event TimerHandler TimerResumedEvent;
	public event TimerTikHandler TimerStoppedEvent;

	public event TimerTikHandler TimerTikkedEvent;
	public event TimerHandler TimerEndedEvent;

	private ConCoroutines concoroutines;

	public bool Running { get; private set; }
	private bool timerWorking = false;

	public int TimesToLoop { get; private set; }
	public float SecondsForTik { get; private set; }

	public float SecondsPassedForTik { get; private set; }
	public float TotalSecondsPassed { get; private set; }
	public int TimesLooped { get; private set; }
	


	public Timer(float tikInSeconds, int timesToLoop = 0)
	{
		this.SecondsForTik = tikInSeconds;
		this.TimesToLoop = timesToLoop + 1;
		TimesLooped = 0;
    }

	public void Start()
	{
		if(this.concoroutines == null)
		{
			this.concoroutines = ConfactoryFinder.Instance.Give<ConCoroutines>();
		}

		timerWorking = true;
		Running = true;
		concoroutines.StartCoroutine(TimerLoop(), this);
		if(TimerStartedEvent != null)
		{
			TimerStartedEvent();
        }
	}

	public void Resume()
	{
		if (timerWorking)
		{
			Running = true;
			concoroutines.StartCoroutine(TimerLoop(), this);

			if(TimerResumedEvent != null)
			{
				TimerResumedEvent();
			}
		}
    }

	public void Reset(bool startAfterReset = false)
	{

		TimesLooped = 0;
		Stop();
		if (startAfterReset)
		{
			Start();
		}
		
    }

	public void Stop()
	{
		Running = false;
		timerWorking = false;
		SecondsPassedForTik = 0;
		if (concoroutines != null && concoroutines.HasContext(this))
		{
			concoroutines.StopContext(this);
			if (TimerStoppedEvent != null)
			{
				TimerStoppedEvent(TimesLooped);
			}
			this.concoroutines = null;
        }
	}

	public void Pause()
	{
		if (timerWorking)
		{
			Running = false;
			if(TimerPausedEvent != null)
			{
				TimerPausedEvent();
            }
		}
    }

	private IEnumerator TimerLoop()
	{
		while(TimesToLoop > TimesLooped)
		{
			yield return null;
			if (Running)
			{
				SecondsPassedForTik += Time.deltaTime;
				if (SecondsPassedForTik >= SecondsForTik)
				{
					TimesLooped++;
					SecondsPassedForTik = 0;
					if (TimerTikkedEvent != null)
					{
						TimerTikkedEvent(TimesLooped);
					}	
				}
			}
			else
			{
				concoroutines.StopContext(this);
			}
		}
		if(TimerEndedEvent != null)
		{
			TimerEndedEvent();
		}
		Stop();
    }
}
