using UnityEngine;
using System.Collections;

public class WeaponHolder {

	public WeaponInfo CurrentWeapon { get; private set; }

	private Transform holderTransform;
	private DamageHitBox hitBox;
	private ObjectPicker objectPicker;

    public WeaponHolder(Transform holderTransform, DamageHitBox weaponsHitBox, ObjectPicker objectPicker)
	{
		this.holderTransform = holderTransform;
		this.hitBox = weaponsHitBox;
		this.objectPicker = objectPicker;
		weaponsHitBox.HitBoxClashEvent += OnHitBoxClashEvent;
    }

	public void PickUpWeapon(bool dropHoldingWeapon = true)
	{
			PickUpInfo infoPickUp = objectPicker.PickUpObject<Weapon>(Vector2.up, holderTransform.position, 1f, true);
			Weapon weapon;
			if (infoPickUp.objectPickedUp != null)
			{
				weapon = infoPickUp.objectPickedUp.GetComponent<Weapon>();
				if (weapon != null)
				{
					if (dropHoldingWeapon)
					{
						DropWeapon(true);
					}
					GainWeapon(weapon.WeaponInfo);
					GameObject.Destroy(weapon.gameObject);
				}
			}
	}

	public void GainWeapon(WeaponInfo weapon)
	{
		CurrentWeapon = weapon;
		SetWeaponHitbox(false);
    }


	public void SetWeaponHitbox(bool attacking)
	{
		hitBox.gameObject.SetActive(false);
		if (CurrentWeapon != null)
		{
			if (attacking)
			{
				hitBox.HitType = CurrentWeapon.WeaponAttackType;
			}
			else
			{
				hitBox.HitType = CurrentWeapon.WeaponIdleType;
			}
		}
		else
		{
			if (attacking)
			{
				hitBox.HitType = DamageHitBox.HitTypes.Ko;
			}
			else
			{
				hitBox.HitType = DamageHitBox.HitTypes.None;
			}
		}

		hitBox.gameObject.SetActive(true);
	}

	public Weapon DropWeapon(bool dropObject = true, Vector3? spawnOffset = null)
	{
		GameObject dropWeaponObject = null;
		if (CurrentWeapon != null)
		{
			if (dropObject)
			{
				Vector3 spawnPos = holderTransform.position; //(spawnOffset.HasValue) ? spawnOffset.Value : holderTransform.position;
				if(spawnOffset.HasValue)
				{
					spawnPos += spawnOffset.Value;
				}
                dropWeaponObject = GameObject.Instantiate(WeaponFactory.GetWeaponGameObject(CurrentWeapon.weapon), spawnPos, Quaternion.identity) as GameObject;
			}
			CurrentWeapon = null;
		}
		SetWeaponHitbox(false);
        return (dropWeaponObject == null) ? null : dropWeaponObject.GetComponent<Weapon>();
	}

	private void OnHitBoxClashEvent(DamageHitBox ownBox, DamageHitBox otherBox)
	{
		if (ownBox.HitType != DamageHitBox.HitTypes.None && otherBox.HitType != DamageHitBox.HitTypes.None)
		{
			if (ownBox.HitType != DamageHitBox.HitTypes.Idle)
			{
				if (ownBox.HitType == otherBox.HitType)		// if we clash and we are both not idle then do the clash effect
				{
					Debug.Log("Clash");
				}
				else if (ownBox.HitType > otherBox.HitType) // if we are both not idle and mine is higher then his then disarm me (ko disarms kill weapons in clash)
				{
					Disarm(ownBox, otherBox);
                }
			}
			else if (otherBox.HitType != DamageHitBox.HitTypes.Idle) // if mine is idle and the other is not then disarm me
			{
				Disarm(ownBox, otherBox);
            }
		}
	}

	private void Disarm(DamageHitBox ownBox, DamageHitBox otherBox, float forceMod = 1)
	{
		if (CurrentWeapon != null)
		{
			float xDiffWeapons = otherBox.transform.position.x - ownBox.transform.position.x;
			float force = 6f;
			if (otherBox.HitType == DamageHitBox.HitTypes.Kill)
			{
				force *= 1.5f;
			}
			force *= forceMod;
			Disarm(new Vector2(-Mathf.Sign(xDiffWeapons), 0.5f), force);
        }
	}

	public void Disarm(Vector2 direction, float velocity)
	{
		if (CurrentWeapon != null)
		{
			Weapon weapon = DropWeapon(true, new Vector2(0, 0.5f));
			weapon.RigidbodyItem.velocity += direction.normalized * velocity;
		}
	}
}
