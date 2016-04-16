using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class Weapon : PickAbleObject
{

	public WeaponInfo WeaponInfo { get; private set; }
	public Rigidbody2D RigidbodyItem { get { return rigidbodyItem; } }
	public float WeaponHurtVelocity { get { return weaponHurtVelocity; } }

	[SerializeField]
	private float weaponHurtVelocity = 14;
	[SerializeField]
	private float weight = 2.5f;
	[SerializeField]
	private Rigidbody2D rigidbodyItem;
	[SerializeField]
	private DamageHitBox hitBoxItem;
	[SerializeField]
	private DamageHitBox.HitTypes attackWeapon;
	[SerializeField]
	private DamageHitBox.HitTypes idleWeapon;
	[SerializeField]
	private WeaponFactory.AllWeapons weapon;

	protected void Awake()
	{
		WeaponInfo = new WeaponInfo(ItemId, idleWeapon, attackWeapon, weapon);
		hitBoxItem.CollisionEvent += OnCollisionEvent;
		rigidbodyItem.gravityScale = weight;
	}

	public void SetHitboxItem(bool attacking)
	{
		if (attacking)
		{
			hitBoxItem.HitType = attackWeapon;
		}
		else
		{
			hitBoxItem.HitType = DamageHitBox.HitTypes.None;
		}
	}

	protected void Update()
	{
		if (RigidbodyItem.velocity.magnitude >= weaponHurtVelocity)
		{
			SetHitboxItem(true);
		}
		else if (hitBoxItem.HitType == attackWeapon)
		{
			SetHitboxItem(false);
		}
	}

	private void OnCollisionEvent(DamageHitBox ownHitbox, Collider2D otherCollider)
	{
		if (gameObject.GetComponent<Collider2D>() != otherCollider)
		{
			if (otherCollider.GetComponent<Character>() != null)
			{
				RigidbodyItem.velocity = new Vector2(0, 0);
			}
			else if (RigidbodyItem.velocity.magnitude >= weaponHurtVelocity)
			{
				RigidbodyItem.velocity = RigidbodyItem.velocity.normalized * (weaponHurtVelocity - 1);
			}
		}
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
