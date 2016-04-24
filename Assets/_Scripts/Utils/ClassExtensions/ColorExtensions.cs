using UnityEngine;
using System.Collections;

public static class ColorExtensions
{
	public static Color AlphaVersion(this Color color, float alpha)
	{
		Color c = color;
		return new Color(c.r, c.g, c.b, alpha);
	}
}
