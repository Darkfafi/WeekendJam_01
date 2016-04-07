using UnityEngine;
using System.Collections;
using Confactory;
using System;
using System.Collections.Generic;
using System.Linq;

public class ConGlobalEvents : IConfactory
{
	public delegate void GlobalDelegate(GlobalEvent eventGiven);

	private Dictionary<string, List<GlobalDelegate>> currentListeners = new Dictionary<string, List<GlobalDelegate>>();

	public IConfactory ConStruct()
	{
		return null;
	}

	public void ConClear()
	{

	}

	public void Dispatch(GlobalEvent gEvent)
	{
		List<GlobalDelegate> allDelegatesToTrigger = currentListeners[gEvent.eventName];
		foreach(GlobalDelegate d in allDelegatesToTrigger)
		{
			d.Invoke(gEvent);
		}
	}

	public void AddListener<T>(string eventName, GlobalDelegate globalDelegate)
	{
		List<GlobalDelegate> allDelegates;
		if (!currentListeners.ContainsKey(eventName))
		{
			allDelegates = new List<GlobalDelegate>();
			allDelegates.Add(globalDelegate);
			currentListeners.Add(eventName, allDelegates);
		}
		else
		{
			currentListeners[eventName].Add(globalDelegate);
		}
		
    }

	public void RemoveListener(GlobalDelegate globalDelegate)
	{
		
		foreach(KeyValuePair<string,List<GlobalDelegate>> pair in currentListeners)
		{
			if(pair.Value.Contains(globalDelegate))
			{
				pair.Value.Remove(globalDelegate);
				if(pair.Value.Count == 0)
				{
					currentListeners.Remove(pair.Key);
				}
				break;
			}
		}
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}
}
public class GlobalEvent
{
	public string eventName { get; private set; }
	public object[] parameters { get; private set; }

	public GlobalEvent(string name, object[] parameters)
	{
		this.eventName = name;
		this.parameters = parameters;
	}
}
