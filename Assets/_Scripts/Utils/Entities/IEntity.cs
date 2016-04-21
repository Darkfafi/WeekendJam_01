// Created by | Ramses Di Perna | 21-04-2016
using UnityEngine;
using System.Collections;
using System;

namespace Ramses.Entities
{
	public interface IEntity : IDisposable
	{
		void AddTag(string tag);
		void RemoveTag(string tag);
		bool HasTag(string tag);
	}
}