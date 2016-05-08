using UnityEngine;
using System.Collections;

public static class ArrayExtensions {

	public static int GetLoopIndex<T>(this T[] array, int index)
	{
		if (index > array.Length - 1)
		{
			index = 0;
		}
		else if (index < 0)
		{
			index = array.Length - 1;
		}
		return index;
	}

	public static int GetIndexOf<T>(this T[] array, T value)
	{
		return System.Array.IndexOf(array, value);
	}
}
