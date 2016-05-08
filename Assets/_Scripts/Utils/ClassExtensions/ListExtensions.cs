using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListExtensions
{
	public static int GetLoopIndex<T>(this List<T> list, int index)
	{
		if (index > list.Count - 1)
		{
			index = 0;
		}
		else if (index < 0)
		{
			index = list.Count - 1;
		}
		return index;
	}
}
