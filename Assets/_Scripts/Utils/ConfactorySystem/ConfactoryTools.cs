// Created by | Ramses Di Perna | 7-04-2016

using UnityEngine;
using System.Collections;

namespace Ramses.Confactory
{
	public static class ConfactoryTools
	{
		public static GameObject CreateConGameObject(string name)
		{
			GameObject go = new GameObject(name);
			SetObjectWithConGameObjectSettings(go);
            return go;
		}

		public static void SetObjectWithConGameObjectSettings(GameObject gameObject, bool staticObject = true)
		{
			gameObject.name = "<" + gameObject.name + ">";
			gameObject.isStatic = true;
			GameObject.DontDestroyOnLoad(gameObject);
        }
	}
}
