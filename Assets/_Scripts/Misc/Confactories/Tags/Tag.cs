using UnityEngine;
using System.Collections;
using Confactory;
using System.Collections.Generic;

public class Tag : MonoBehaviour
{
	public ConTags.TagList[] ObjectTags { get { return objectTags.ToArray(); } }
	[SerializeField]
	private List<ConTags.TagList> objectTags;


	public bool IsTag(ConTags.TagList tag)
	{
		return objectTags.Contains(tag);
	}

	public void Awake()
	{
		ConfactoryFinder.Instance.Give<ConTags>().Register(this);
	}

	public void OnDestroy()
	{
		ConfactoryFinder.Instance.Give<ConTags>().UnRegister(this);
	}
}
