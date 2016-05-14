using UnityEngine;
using System.Collections;

public static class ArrayExtensions {

	public static int GetLoopIndex<T>(this T[] array, int index)
	{
		if (array.Length > 0)
		{
			while (index > array.Length - 1)
			{
				index -= array.Length;
			}
			while (index < 0)
			{
				index += array.Length;
			}
		}
		else
		{
			index = 0;
		}
		return index;
	}

	public static int GetClampedIndex<T>(this T[] array, int index)
	{
		if (index > array.Length - 1)
		{
			index = array.Length - 1;
		}
		else if (index < 0)
		{
			index = 0;
		}
		return index;
	}

	public static int GetIndexOf<T>(this T[] array, T value)
	{
		return System.Array.IndexOf(array, value);
	}

	public static T GetLoop<T>(this T[] array, int index)
	{
		return array[array.GetLoopIndex(index)];
	}

	public static T GetClamped<T>(this T[] array, int index)
	{
		return array[array.GetClampedIndex(index)];
	}

}
