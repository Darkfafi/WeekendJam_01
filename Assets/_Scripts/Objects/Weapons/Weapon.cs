using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class Weapon : PickAbleObject {
	
	public WeaponInfo WeaponInfo { get; private set; }
	public DamageHitBox HitBoxItem { get { return hitBoxItem; } }

	[SerializeField] private DamageHitBox hitBoxItem;
	[SerializeField] private DamageHitBox.HitTypes attackWeapon;
	[SerializeField] private DamageHitBox.HitTypes idleWeapon;
	[SerializeField] private WeaponFactory.AllWeapons weapon;

	protected void Awake()
	{
		WeaponInfo = new WeaponInfo(ItemId, idleWeapon, attackWeapon, weapon);
    }
}

public class WeaponInfo
{
	public string ItemId { get; private set; }
	public DamageHitBox.HitTypes WeaponAttackType { get; private set; }
	public DamageHitBox.HitTypes WeaponIdleType { get; private set; }
	public WeaponFactory.AllWeapons weapon { get; private set; }

	public WeaponInfo(string itemId, DamageHitBox.HitTypes weaponIdleType, DamageHitBox.HitTypes weaponAttackType, WeaponFactory.AllWeapons weapon)
	{
		this.ItemId = itemId;
		this.WeaponIdleType = weaponIdleType;
		this.WeaponAttackType = weaponAttackType;
		this.weapon = weapon;
	}
}


