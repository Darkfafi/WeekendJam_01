using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;

public class ConSelectedGameRules : IConfactory {

	private BaseGameRules selectedGameRules = null;

	public void SetSelectedGameRules(BaseGameRules gameRules)
	{
		selectedGameRules = gameRules;
    }

	public BaseGameRules GetSelectedGameRules()
	{
		return selectedGameRules;
	}

	public void ConClear()
	{

	}

	public void ConStruct()
	{

	}

	public void OnSceneSwitch(int newSceneIndex)
	{

	}
}
