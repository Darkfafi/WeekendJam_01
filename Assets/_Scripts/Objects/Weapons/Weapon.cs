using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class Weapon : PickAbleObject {

	public WeaponInfo WeaponInfo { get; private set; }
	[SerializeField] private bool killWeapon = true;
	[SerializeField] private WeaponFactory.AllWeapons weapon;

	protected void Awake()
	{
		WeaponInfo = new WeaponInfo(ItemId, killWeapon, weapon);
    }
}

public class WeaponInfo
{
	public string ItemId { get; private set; }
	public bool KillWeapon { get; private set; }
	public WeaponFactory.AllWeapons weapon { get; private set; }

	public WeaponInfo(string itemId, bool killWeapon, WeaponFactory.AllWeapons weapon)
	{
		this.ItemId = itemId;
		this.KillWeapon = killWeapon;
		this.weapon = weapon;
	}
}


