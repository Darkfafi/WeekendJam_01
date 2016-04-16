using UnityEngine;
using System.Collections;

public class Layers {

	public const int PLAYERS = 8;
	public const int OBJECTS = 9;
	public const int HITBOXES = 10;

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
		LayerMask ignoreReturn = -1;
		if(layers.Length > 0)
		{
			ignoreReturn = new LayerMask();
        }
		foreach(int layer in layers)
		{
			ignoreReturn |= (1 << layer);
        }
		ignoreReturn = ~ignoreReturn;
		return ignoreReturn;
    }

	public static int LayerMaskSeeOnly(int[] layers)
	{
		LayerMask seeReturn = -1;
		if (layers.Length > 0)
		{
			seeReturn = new LayerMask();
		}

		foreach (int layer in layers)
		{
			seeReturn |= (1 << layer);
		}
		return seeReturn;
	}
}
