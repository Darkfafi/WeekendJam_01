// Created by | Ramses Di Perna | 27-03-2016

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Ramses.Confactory
{
	/// <summary>
	/// This class is responsible for Creating, managing and destroying Confactories.
	/// </summary>
	public class ConfactoryFinder : IConfactoryFinder
	{
		private Dictionary<Type, IConfactory> activeConfectories = new Dictionary<Type, IConfactory>();

		public static ConfactoryFinder Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ConfactoryFinder();
				}
				return instance;
			}
		}

		private static ConfactoryFinder instance;
		private SceneSwitchChecker sceneChecker = null;

		private List<Type> inCreationTypes = new List<Type>();
		private List<Type> inDeletionTypes = new List<Type>();

		public T Give<T>() where T : IConfactory
		{
			return (T)Give(typeof(T));
		}

		public IConfactory Give(Type t)
		{
			IConfactory confectory = null;
			if (activeConfectories.ContainsKey(t))
			{
				confectory = activeConfectories[t];
			}
			else
			{
				if (!inCreationTypes.Contains(t))
				{
					if(sceneChecker == null)
					{
						CreateSceneSwitchChecker();
					}
					inCreationTypes.Add(t);
					confectory = CreateConfectory(t);
					inCreationTypes.Remove(t);
				}
				else
				{
					Debug.LogWarning("Confectory " + t.Name + " already in construction, don't try to access it!");
				}
			}

			return confectory;
		}

		public void Delete<T>() where T : IConfactory
		{
			Delete(typeof(T));
        }

		public void Delete(Type t)
		{
			if(activeConfectories.ContainsKey(t))
			{
				if (!inDeletionTypes.Contains(t))
				{
					inDeletionTypes.Add(t);
					IConfactory confactory = activeConfectories[t];
					activeConfectories.Remove(t);
					confactory.ConClear();
					if(confactory.GetType().IsSubclassOf(typeof(MonoBehaviour)))
                    {
						GameObject.Destroy((confactory as MonoBehaviour).gameObject);
                    }
					inDeletionTypes.Remove(t);
					if(activeConfectories.Count == 0)
					{
						DeleteSceneSwitchChecker();
                    }
				}
				else
				{
					Debug.LogWarning("Confectory " + t.Name + " already in process of deletion, don't try to delete it!");
				}
            }
		}

		private IConfactory CreateConfectory(Type t)
		{
			IConfactory confectory;

			if(t.IsSubclassOf(typeof(MonoBehaviour)))
			{
				confectory = (IConfactory)ConfactoryTools.CreateConGameObject(t.Name).AddComponent(t);
			}
			else {
				confectory = (IConfactory)Activator.CreateInstance(t); 
			}

			confectory.ConStruct();

			activeConfectories.Add(confectory.GetType(), confectory);
			return confectory;
		}

		private void DeleteSceneSwitchChecker()
		{
			if (sceneChecker != null)
			{
				sceneChecker.SceneSwitchEvent -= OnSceneSwitchedEvent;
				GameObject.Destroy(sceneChecker.gameObject);
				sceneChecker = null;
			}
		}

		private void CreateSceneSwitchChecker()
		{
			if (sceneChecker == null)
			{
				GameObject gObject = ConfactoryTools.CreateConGameObject("(Confinder)SceneSwitchChecker");
				sceneChecker = gObject.AddComponent<SceneSwitchChecker>();
				sceneChecker.SceneSwitchEvent += OnSceneSwitchedEvent;
			}
        }

		private void OnSceneSwitchedEvent(int currentLevel)
		{
			foreach(KeyValuePair<Type, IConfactory> confactoryPair in activeConfectories)
			{
				confactoryPair.Value.OnSceneSwitch(currentLevel);
            }
		}
    }
}
