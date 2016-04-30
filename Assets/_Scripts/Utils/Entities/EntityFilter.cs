using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Entities;
using System;

public class EntityFilter : IDisposable
{
	public delegate void EntityHandler(IEntity entity);
	public event EntityHandler FilteredEntityAddedEvent;
	public event EntityHandler FilteredEntityRemovedEvent;

	private string[] tags;
	private List<IEntity> allTrackingEntities;

	private ConEntityDatabase database;

	public EntityFilter(string[] tags, ConEntityDatabase database)
	{
		this.tags = tags;
		this.database = database;
		allTrackingEntities = new List<IEntity>(database.GetEntities(tags));

		database.EntityRegisteredEvent -= OnEntityRegister;
		database.EntityUnRegisteredEvent -= OnEntityOnRegister;
		database.EntityAddedTagEvent -= OnAddedTag;
		database.EntityRemovedTagEvent -= OnRemovedTag;

		database.EntityRegisteredEvent += OnEntityRegister;
		database.EntityUnRegisteredEvent += OnEntityOnRegister;
		database.EntityAddedTagEvent += OnAddedTag;
		database.EntityRemovedTagEvent += OnRemovedTag;
    }

	public IEntity[] GetAllFilteredEntities()
	{
		return allTrackingEntities.ToArray();
    }

	public void Dispose()
	{
		database.EntityRegisteredEvent -= OnEntityRegister;
		database.EntityUnRegisteredEvent -= OnEntityOnRegister;
		database.EntityAddedTagEvent -= OnAddedTag;
		database.EntityRemovedTagEvent -= OnRemovedTag;
		
		database = null;
		allTrackingEntities = null;
		tags = null;
    }

	private void OnEntityRegister(IEntity entity)
	{
		if(entity.HasAnyOfTags(tags) && !allTrackingEntities.Contains(entity))
		{
			AddEntity(entity);
        }
	}

	private void OnEntityOnRegister(IEntity entity)
	{
		if (allTrackingEntities.Contains(entity))
		{
			RemoveEntity(entity);
		}
	}

	private void OnAddedTag(IEntity entity, string tag)
	{
		if (entity.HasTag(tag) && !allTrackingEntities.Contains(entity))
		{
			AddEntity(entity);
		}
	}

	private void OnRemovedTag(IEntity entity, string tag)
	{
		if (allTrackingEntities.Contains(entity))
		{
			RemoveEntity(entity);
		}
	}

	private void AddEntity(IEntity entity)
	{
		if(FilteredEntityAddedEvent != null)
		{
			FilteredEntityAddedEvent(entity);
        }
	}

	private void RemoveEntity(IEntity entity)
	{
		if (FilteredEntityRemovedEvent != null)
		{
			FilteredEntityRemovedEvent(entity);
        }
	}
}
