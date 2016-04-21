// Created by | Ramses Di Perna | 27-03-2016

using UnityEngine;
using System.Collections;
namespace Ramses.Confactory
{
	/// <summary>
	/// The IConfactory interface is an interface which allowes for a singleton to be made with its own Constructor and variables
	/// If the class which is given this interface to is a Monobehaviour, then the GameObject will be made automaticly when the Confactory created
	/// The Confactories are only created when they are needed. The ConfactoryFinder creates the confactories when it is asked to give one to the asker.
	/// There is always only 1 Confactory in the world.
	/// </summary>
	public interface IConfactory
	{
		/// <summary>
		/// WARNING: Don't call this method!
		/// This Method will be called when the Confactory is called when he is created (Which happens when it is called for the first time)
		/// </summary>
		void ConStruct(); // This will be called when the Confector is Given for the first time by the locator.
		/// <summary>
		/// WARNING: Don't call this method!
		/// This Method will be called when the Confactory is deleted by the Delete method in the ConfactoryFinder
		/// If the Confactory is a Monobehaviour, the GameObject created for it will automaticly be removed after this method.
		/// </summary>
		void ConClear();
		/// <summary>
		/// WARNING: Don't call this method!
		/// This Method will be called when the Scene is switched.
		/// </summary>
		void OnSceneSwitch(int newSceneIndex);
	}
}
