using UnityEngine;
using System.Collections;

public static class TimerUtils {

	public enum TimeVisibleRange
	{
		Seconds = 0,
		Minutes = 1,
		Hours = 2
	}

	public static string MinutesToClockString(float timeInMinutes, TimeVisibleRange visibleRange = TimeVisibleRange.Minutes)
	{
		return SecondsToClockString(timeInMinutes * 60, visibleRange);
	}

	public static string SecondsToClockString(float timeInSeconds, TimeVisibleRange visibleRange = TimeVisibleRange.Minutes)
	{
		string timeString = "";
		int minuteCounter = 0;
		int hourCounter = 0;

		while(timeInSeconds >= 3600)
		{
			timeInSeconds -= 3600;
			hourCounter++;
		}

		while(timeInSeconds >= 60)
		{
			timeInSeconds -= 60;
			minuteCounter++;
		}

		if(hourCounter > 0)
		{
			timeString +=  (hourCounter < 10 ? "0" : "") + hourCounter.ToString() + ":";
		}
		else if(visibleRange == TimeVisibleRange.Hours)
		{
			timeString += "00:";
		}

		if(minuteCounter > 0)
		{
			timeString += (minuteCounter < 10 ? "0" : "") + minuteCounter.ToString() + ":";
		}
		else if(visibleRange >= TimeVisibleRange.Minutes)
		{
			timeString += "00:";
		}

		if(timeInSeconds > 0)
		{
			timeString += (timeInSeconds < 10 ? "0" : "") + timeInSeconds.ToString();
		}
		else
		{
			timeString += (minuteCounter > 0 || hourCounter > 0 || visibleRange >= TimeVisibleRange.Minutes) ? "00" : "0";
		}


		return timeString;
	}
}
