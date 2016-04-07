// Created by | Ramses Di Perna | 27-03-2016

using UnityEngine;
using System.Collections;
namespace Confactory
{
	public interface IConfactory
	{
		IConfactory ConStruct(); // This will be called when the Confector is Given for the first time by the locator
		void ConClear();
		void OnSceneSwitch(int newSceneIndex);
	}
}
