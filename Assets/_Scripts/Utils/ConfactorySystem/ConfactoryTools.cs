// Created by | Ramses Di Perna | 7-04-2016

using UnityEngine;
using System.Collections;

namespace Confactory
{
	public static class ConfactoryTools
	{
		public static GameObject CreateConGameObject(string name)
		{
			GameObject go = new GameObject("<" + name + ">");
			GameObject.DontDestroyOnLoad(go);
			return go;
		}
	}
}
