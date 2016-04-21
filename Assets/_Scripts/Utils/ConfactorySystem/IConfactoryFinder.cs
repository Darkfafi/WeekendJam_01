// Created by | Ramses Di Perna | 27-03-2016

using UnityEngine;
using System.Collections;
using System;
namespace Ramses.Confactory
{
	public interface IConfactoryFinder
	{

		T Give<T>() where T : IConfactory;
		IConfactory Give(Type t);
	}
}