// Created by | Ramses Di Perna | 21-04-2016
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Ramses.Entities
{
	public class Entity : IEntity
	{
		public event TagHandler TagAddedEvent;
		public event TagHandler TagRemovedEvent;

		protected ConEntityDatabase database;
		private List<string> allTags = new List<string>();
		private IEntity trueEntity = null;

		/// <summary>
		/// When realEntity is left null, the Entity will register itself. Else it will register whoever is the realEntity
		/// WARNING: Don't use the RealEntity if you are inheriting from this class.
		/// </summary>
		public Entity(IEntity realEntity = null, string[] tags = null)
		{
			database = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConEntityDatabase>();
			this.trueEntity = (realEntity == null) ? this : realEntity;
			if (tags != null)
			{
				allTags = new List<string>(tags);
			}
			RegisterToDatabase(trueEntity);
        }

		public virtual void AddTag(string tag)
		{
			if (!allTags.Contains(tag))
			{
				allTags.Add(tag);
				if(TagAddedEvent != null)
				{
					TagAddedEvent(trueEntity, tag);
                }
			}
		}

		public virtual void Dispose()
		{
			database.UnRegisterEntity(trueEntity);
			database = null;
		}

		public virtual bool HasTag(string tag)
		{
			return allTags.Contains(tag);
		}

		public virtual void RemoveTag(string tag)
		{
			if (allTags.Contains(tag))
			{
				allTags.Remove(tag);
				if(TagRemovedEvent != null)
				{
					TagRemovedEvent(trueEntity, tag);
                }
			}
		}

		public virtual bool HasAnyOfTags(string[] tags)
		{
			if (tags.Length > 0)
			{
				for(int i = 0; i < tags.Length; i++)
				{
					if(HasTag(tags[i]))
					{
						return true;
					}
				}
			}

			return false;
		}

		private void RegisterToDatabase(IEntity entity)
		{
			database.RegisterEntity(entity);
		}
	}
}