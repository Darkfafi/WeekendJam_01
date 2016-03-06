using UnityEngine;
using System.Collections;

public class Layers {

	public const int PLAYERS = 8;
	public const int OBJECTS = 9;


	public static int LayerMaskIgnore(int layer)
	{
		return ~(1 << layer);
	}
	public static int LayerMaskSeeOnly(int layer)
	{
		return (1 << layer);
	}

	public static int LayerMaskIgnore(int[] layers)
	{
		int ignoreReturn = -1;
		foreach(int layer in layers)
		{
			ignoreReturn += ~(1 << layer);
        }
		return ignoreReturn;
    }
	public static int LayerMaskSeeOnly(int[] layers)
	{
		int seeReturn = -1;
		foreach (int layer in layers)
		{
			seeReturn += (1 << layer);
		}
		return seeReturn;
	}
}
