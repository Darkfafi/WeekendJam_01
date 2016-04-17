using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ColorHandler {

	public enum Colors
	{
		Red,
		Green,
		Yellow,
		Purple,
		Cyan,
		Orange
	}

	private List<Colors> availableColors = new List<Colors>((Colors[])Enum.GetValues(typeof(Colors)));

	public ColorHandler()
	{

    }

	public Colors[] GetAvailableColors()
	{
		return availableColors.ToArray();
	}

	public Color GetColor(Colors color, bool use = false)
	{
		if(use)
		{
			if (availableColors.Contains(color))
			{
				availableColors.Remove(color);
				return ColorsToColor(color);
			}
			else
			{
				Debug.LogError("Color: " + color + " not available!");
				return Color.black;
			}
		}
		else
		{
			return ColorsToColor(color);
		}

		
    }

	public void StopUsingColor(Colors color)
	{
		List<Colors> check = new List<Colors>((Colors[])Enum.GetValues(typeof(Colors)));
		if(check.Contains(color) && !availableColors.Contains(color))
		{
			availableColors.Add(color);
		}
	}

	public static Color ColorsToColor(Colors color)
	{
		switch(color)
		{
			case Colors.Red:
				return Color.red;
			case Colors.Purple:
				return new Color(0.498f, 0,1);
			case Colors.Green:
				return Color.green;
			case Colors.Yellow:
				return Color.yellow;
			case Colors.Cyan:
				return Color.cyan;
			case Colors.Orange:
				return new Color(1, 0.6f, 0.2f);
			default:
				return Color.black;
		}
	}
}
