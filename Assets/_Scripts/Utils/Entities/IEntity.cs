// Created by | Ramses Di Perna | 21-04-2016
using UnityEngine;
using System.Collections;
using System;

namespace Ramses.Entities
{
	public delegate void TagHandler(IEntity entity, string tag);

	public interface IEntity : IDisposable
	{
		event TagHandler TagAddedEvent;
		event TagHandler TagRemovedEvent;

		void AddTag(string tag);
		void RemoveTag(string tag);
		bool HasTag(string tag);
		bool HasAnyOfTags(string[] tags);
	}
}