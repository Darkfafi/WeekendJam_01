// Created by | Ramses Di Perna | 21-04-2016
using UnityEngine;
using System.Collections;
using System;

namespace Ramses.Entities
{
	/// <summary>
	/// The component version of the 'Entity' class
	/// WARNING: If you want to use Awake, please override it and call the base method, else the class will break!
	/// </summary>
	public class MonoEntity : MonoBehaviour, IEntity
	{
		private Entity entity = null;

		protected virtual void Awake()
		{
			entity = new Entity(this);
		}

		public void AddTag(string tag)
		{
			entity.AddTag(tag);
		}

		public void Dispose()
		{
			entity.Dispose();
			Destroy(this.gameObject);
		}

		public bool HasTag(string tag)
		{
			return entity.HasTag(tag);
		}

		public void RemoveTag(string tag)
		{
			entity.RemoveTag(tag);
		}

		protected virtual void OnDestroy()
		{
			this.Dispose();
		}
	}
}