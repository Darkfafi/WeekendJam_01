using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class Weapon : PickAbleObject {

	public WeaponInfo WeaponInfo { get; private set; }
	[SerializeField] private DamageHitBox.HitTypes killWeapon;
	[SerializeField] private WeaponFactory.AllWeapons weapon;

	protected void Awake()
	{
		WeaponInfo = new WeaponInfo(ItemId, killWeapon, weapon);
    }
}

public class WeaponInfo
{
	public string ItemId { get; private set; }
	public DamageHitBox.HitTypes WeaponHitType { get; private set; }
	public WeaponFactory.AllWeapons weapon { get; private set; }

	public WeaponInfo(string itemId, DamageHitBox.HitTypes weaponHitType, WeaponFactory.AllWeapons weapon)
	{
		this.ItemId = itemId;
		this.WeaponHitType = weaponHitType;
		this.weapon = weapon;
	}
}


