// Created by | Ramses Di Perna | 06-04-2016

using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;
using System.Collections.Generic;
public class ConGameInputBindings : IConfactory
{
	public delegate void BindingsHandler(BindingTypes inputType, InputItem[] inputItems);
	public event BindingsHandler BindingsAddedEvent;

	public enum BindingTypes
	{
		None,
		Keyboard01,
		Keyboard02,

		Joystick01,
		Joystick02,
		Joystick03,
		Joystick04
	}
	public Dictionary<BindingTypes, InputItem[]> AllBindings { get { return allBindings; } }
	private Dictionary<BindingTypes, InputItem[]> allBindings = new Dictionary<BindingTypes, InputItem[]>();

	public void ConStruct()
	{

	}

	public void ConClear()
	{
		
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}

	public InputItem[] GetBindingsOf(BindingTypes inputType)
	{
		if(allBindings.ContainsKey(inputType))
		{
			return allBindings[inputType];
		}
		return null;
	}

	public void AddBindings(BindingTypes inputType, InputItem[] inputItems)
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
