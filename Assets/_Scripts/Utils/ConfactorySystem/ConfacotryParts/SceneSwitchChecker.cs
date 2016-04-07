using UnityEngine;
using System.Collections;

public class SceneSwitchChecker : MonoBehaviour {

	public delegate void SceneSwitchHandler(int newLevel);
	public event SceneSwitchHandler SceneSwitchEvent;

	void OnLevelWasLoaded(int level)
	{
		if(SceneSwitchEvent != null)
		{
			SceneSwitchEvent(level);
        }
	}
}
