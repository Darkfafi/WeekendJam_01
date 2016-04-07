using UnityEngine;
using System.Collections;
using Confactory;
using System;
using System.Collections.Generic;

public class ConCoroutines : IConfactory
{
	private CoroutineObject coroutineObject;
	private const string NAME_OBJECT = "<Coroutine>:";

	private Dictionary<object, Coroutine> allRunningRoutines = new Dictionary<object, Coroutine>();

	public IConfactory ConStruct()
	{
		coroutineObject = ConfactoryTools.CreateConGameObject(NAME_OBJECT).AddComponent<CoroutineObject>();
		UpdateName();
		return null;
	}

	public void ConClear()
	{
		GameObject.Destroy(coroutineObject.gameObject);
	}

	public void OnSceneSwitch(int newSceneIndex)
	{

	}

	public void StartCoroutine(IEnumerator method, object context)
	{
		if (context != null)
		{
			if (!allRunningRoutines.ContainsKey(context))
			{
				allRunningRoutines.Add(context, coroutineObject.StartCoroutine(method));
			}
			else
			{
				Debug.LogError("There is already a method running on this context");
			}
			UpdateName();
		}
    }
	public void StopContext(object context)
	{
		if(allRunningRoutines.ContainsKey(context))
		{
			coroutineObject.StopCoroutine(allRunningRoutines[context]);
			allRunningRoutines.Remove(context);
			UpdateName();
        }
	}

	private void UpdateName()
	{
		coroutineObject.name = NAME_OBJECT + "("+ allRunningRoutines.Count +")";
	}

	internal class CoroutineObject : MonoBehaviour{}
}
