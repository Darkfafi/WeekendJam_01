// Created by | Ramses Di Perna | 06-04-2016

using UnityEngine;
using System.Collections;
using Confactory;
using System;
using System.Collections.Generic;
public class ConGameInputBindings : MonoBehaviour, IConfactory
{
	public delegate void BindingsHandler(int inputType, InputItem[] inputItems);
	public event BindingsHandler BindingsAddedEvent;

	private Dictionary<int, InputItem[]> allBindings = new Dictionary<int, InputItem[]>();

	public IConfactory ConStruct()
	{
		return null;
	}

	public void ConClear()
	{
		
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}

	public InputItem[] GetBindingsOf(int inputType)
	{
		if(allBindings.ContainsKey(inputType))
		{
			return allBindings[inputType];
		}
		return null;
	}

	public void AddBindings(int inputType, InputItem[] inputItems)
	{
		if (!allBindings.ContainsKey(inputType))
		{
			allBindings.Add(inputType, inputItems);

			if(BindingsAddedEvent != null)
			{
				BindingsAddedEvent(inputType, inputItems);
            }
		}
    }
}
