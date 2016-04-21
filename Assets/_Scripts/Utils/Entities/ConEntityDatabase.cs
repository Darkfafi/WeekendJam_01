// Created by | Ramses Di Perna | 21-04-2016
using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;
using System.Collections.Generic;

namespace Ramses.Entities
{
	/// <summary>
	/// This class is the Database where all the Entities and MonoEntities are stored.
	/// You can find Entities by looking by Type and / or Tag.
	/// An Entity has to be Registered before it can be found in this database.
	/// </summary>
	public class ConEntityDatabase : IConfactory
	{

		public delegate void EntityHandler(IEntity entity);
		public event EntityHandler EntityRegisteredEvent;
		public event EntityHandler EntityUnRegisteredEvent;

		private List<IEntity> allEntities = new List<IEntity>();

		// register section
		/// <summary>
		/// Registers an entity into the database.
		/// Entities Register themselves on creation automaticly.
		/// </summary>
		public void RegisterEntity(IEntity entity)
		{
			if (entity != null)
			{
				if (!allEntities.Contains(entity))
				{
					allEntities.Add(entity);
					if (EntityRegisteredEvent != null)
					{
						EntityRegisteredEvent(entity);
					}
				}
				else
				{
					Debug.LogError("Cannot register " + entity + "because it's already registered!");
				}
			}
			else
			{
				Debug.LogError("Cannot register null entity");
			}
		}
		/// <summary>
		/// UnRegisters an entity from the database.
		/// Entities Unregister themselves on destruction automaticly 
		/// WARNING: Be sure to Dispose() a non MonoEntity whereby it will not be UnRegistered when not cleaned this way.
		/// </summary>
		public void UnRegisterEntity(IEntity entity)
		{
			if (entity != null)
			{
				if (allEntities.Contains(entity))
				{
					allEntities.Remove(entity);
					if (EntityUnRegisteredEvent != null)
					{
						EntityUnRegisteredEvent(entity);
					}
				}
				else
				{
					Debug.LogError("Cannot unregister " + entity + "because it's not registered!");
				}
			}
			else
			{
				Debug.LogError("Cannot unregister null entity");
			}
		}

		// Tag only

		public IEntity[] GetEntities(string tag)
		{
			return GetEntitiesWithTagFromList(allEntities.ToArray(), tag);
		}

		public IEntity GetAnyEntity(string tag)
		{
			return GetEntityWithTagFromList(allEntities.ToArray(), tag);
		}

		// Type only

		public T[] GetEntities<T>() where T : class, IEntity
		{
			return GetEntitiesOfTypeFromList<T>(allEntities.ToArray());
		}

		public T GetAnyEntity<T>() where T : class, IEntity
		{
			return GetEntityOfTypeFromList<T>(allEntities.ToArray());
		}

		// Tag & Type

		public T[] GetEntities<T>(string tag) where T : class, IEntity
		{
			IEntity[] checkEntityList = GetEntitiesWithTagFromList(allEntities.ToArray(), tag);
			return GetEntitiesOfTypeFromList<T>(checkEntityList);
		}

		public T GetAnyEntity<T>(string tag) where T : class, IEntity
		{
			IEntity[] checkEntityList = GetEntitiesWithTagFromList(allEntities.ToArray(), tag);
			return GetEntityOfTypeFromList<T>(checkEntityList);
		}

		// other

		private IEntity[] GetEntitiesWithTagFromList(IEntity[] list, string tag)
		{
			List<IEntity> returns = new List<IEntity>();
			foreach (IEntity entity in list)
			{
				if (entity.HasTag(tag))
				{
					returns.Add(entity);
				}
			}
			return returns.ToArray();
		}

		private IEntity GetEntityWithTagFromList(IEntity[] list, string tag)
		{
			foreach (IEntity entity in list)
			{
				if (entity.HasTag(tag))
				{
					return entity;
				}
			}
			return null;
		}

		private T[] GetEntitiesOfTypeFromList<T>(IEntity[] list) where T : class, IEntity
		{
			List<T> returns = new List<T>();
			foreach (IEntity entity in list)
			{
				if (entity is T)
				{
					returns.Add((T)entity);
				}
			}
			return returns.ToArray();
		}

		private T GetEntityOfTypeFromList<T>(IEntity[] list) where T : class, IEntity
		{
			foreach (IEntity entity in list)
			{
				if (entity is T)
				{
					return (T)entity;
				}
			}

			return null;
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
}