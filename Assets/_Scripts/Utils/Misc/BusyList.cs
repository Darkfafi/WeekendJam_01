using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BusyList {
	public delegate void BusyActionHandler(string busyAction, int layer);
	public delegate void LayerActionHander(int layer);

	public event BusyActionHandler BusyActionAddedEvent;
	public event BusyActionHandler BusyActionRemovedEvent;
	public event LayerActionHander LayerRemovedEvent;
	public event LayerActionHander LayerAddedEvent;

	private Dictionary<int,List<string>> busyActions = new Dictionary<int, List<string>>();

	public void AddBusyAction(string action, int layer = 0)
	{
		if(!busyActions.ContainsKey(layer))
		{
			busyActions.Add(layer, new List<string>());
			if(LayerAddedEvent != null)
			{
				LayerAddedEvent(layer);
            }
        }
		if (!busyActions[layer].Contains(action)) { 
			busyActions[layer].Add(action);
			if(BusyActionAddedEvent != null)
			{
				BusyActionAddedEvent(action, layer);
            }
		}
		else
		{
			Debug.LogWarning("There is already a busyaction called '" + action + "' in this layer.");
		}
	}
	public void RemoveBusyAction(string action)
	{
		List<int> placesToRemoveKey = new List<int>();
		foreach (KeyValuePair<int, List<string>> dicPart in busyActions)
		{
			if (busyActions[dicPart.Key].Contains(action))
			{
				placesToRemoveKey.Add(dicPart.Key);
			}
		}
		foreach(int key in placesToRemoveKey)
		{
			RemoveBusyAction(action, key);
		}
	}
	public void RemoveBusyAction(string action, int layer)
	{
		if (busyActions.ContainsKey(layer))
		{
			if (busyActions[layer].Contains(action))
			{
				busyActions[layer].Remove(action);
				if(BusyActionRemovedEvent != null)
				{
					BusyActionRemovedEvent(action, layer);
                }
            }
			if(busyActions[layer].Count == 0)
			{
				busyActions.Remove(layer);
				if(LayerRemovedEvent != null)
				{
					LayerRemovedEvent(layer);
                }
            }
		}
    }
	public void RemoveBusyAction(int layer)
	{
		if (busyActions.ContainsKey(layer))
		{
			foreach(string action in busyActions[layer])
			{
				RemoveBusyAction(action, layer);
            }
		}
    }
	public bool InBusyAction()
	{
		if(busyActions.Count > 0)
		{
			return true;
		}

		return false;
	}

	public void ClearBusyList()
	{
		busyActions = new Dictionary<int, List<string>>();
    }

	public bool InBusyAction(string action)
	{
        foreach (KeyValuePair<int,List<string>> dicPart in busyActions)
		{
			if (InBusyAction(action, dicPart.Key))
			{
				return true;
			}
		}
		return false;
	}
	public bool InBusyAction(int layer)
	{
		if (busyActions.ContainsKey(layer))
		{
			if (busyActions[layer].Count > 0)
			{
				return true;
			}
		}
		return false;
	}
	public bool InBusyAction(string action, int layer)
	{
		if (InBusyAction(layer))
		{
			foreach(string actionInList in busyActions[layer])
			{
				if(actionInList == action)
				{
					return true;
				}
			}
		}
		return false;
	}
}
