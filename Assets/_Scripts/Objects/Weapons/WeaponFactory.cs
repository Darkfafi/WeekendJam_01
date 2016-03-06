using UnityEngine;
using System.Collections;

public class WeaponFactory {

	private const string WEAPONS_PATH = "Weapons/";

	public enum AllWeapons
	{
		Spear
	}

	public static GameObject GetWeaponGameObject(AllWeapons weapon)
	{
		string resourcePath = WEAPONS_PATH;
		switch(weapon)
		{
			case AllWeapons.Spear:
				resourcePath += "Spear";
                break;
		}

		return Resources.Load<GameObject>(resourcePath) as GameObject;
	}
}
