using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class ImageExtensions {

	public static Color SetAlpha(this Image image, float alpha)
	{
		image.color = image.color.AlphaVersion(alpha);
		return image.color;
	}
}
