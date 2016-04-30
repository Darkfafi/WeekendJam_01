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
		public event TagHandler TagAddedEvent;
		public event TagHandler TagRemovedEvent;

		private Entity entity = null;

		[SerializeField] private string[] entityTags;

		protected virtual void Awake()
		{
			entity = new Entity(this, entityTags);

			entity.TagRemovedEvent -= OnTagRemovedEvent;
			entity.TagAddedEvent -= OnTagAddedEvent;

			entity.TagRemovedEvent += OnTagRemovedEvent;
			entity.TagAddedEvent += OnTagAddedEvent;
		}

		public void AddTag(string tag)
		{
			entity.AddTag(tag);
		}

		public void Dispose()
		{
			entity.TagRemovedEvent -= OnTagRemovedEvent;
			entity.TagAddedEvent -= OnTagAddedEvent;
			entity.Dispose();
			entity = null;
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

		private void OnTagAddedEvent(IEntity entity, string tag)
		{
			if(TagRemovedEvent != null)
			{
				TagRemovedEvent(this, tag);
            }
		}

		private void OnTagRemovedEvent(IEntity entity, string tag)
		{
			if (TagAddedEvent != null)
			{
				TagAddedEvent(this, tag);
			}
		}

		public bool HasAnyOfTags(string[] tags)
		{
			return entity.HasAnyOfTags(tags);
		}
	}
}