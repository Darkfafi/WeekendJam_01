using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Confactory;
using System;

public class ConTags : IConfactory{

	public enum TagList
	{
		Spawnpoint
	}

	private List<Tag> allTags = new List<Tag>();

	public GameObject[] GetTagObjects(TagList tag)
	{
		List<GameObject> allObjects = new List<GameObject>();

		foreach(Tag tagItem in allTags)
		{
			if(tagItem.IsTag(tag))
			{
				allObjects.Add(tagItem.gameObject);
			}
		}

		return allObjects.ToArray();
	}

	public void Register(Tag tag)
	{
		if (!allTags.Contains(tag))
		{
			allTags.Add(tag);
		}
	}

	public void UnRegister(Tag tag)
	{
		if (allTags.Contains(tag))
		{
			allTags.Remove(tag);
		}
	}

	public void ConStruct()
	{
		
	}

	public void ConClear()
	{
		
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}
}