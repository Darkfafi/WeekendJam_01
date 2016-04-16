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
			if (ownBox.HitType == otherBox.HitType && ownBox.HitType != DamageHitBox.HitTypes.Idle)
			{
				Debug.Log("Clash");
			}
			else if (ownBox.HitType == DamageHitBox.HitTypes.Idle && otherBox.HitType != DamageHitBox.HitTypes.Idle)
			{
				Weapon weapon = DropWeapon(true,new Vector2(0,0.5f));
				float xDiffWeapons = otherBox.transform.position.x - ownBox.transform.position.x;
				float force = 4f;
				if(otherBox.HitType == DamageHitBox.HitTypes.Kill)
				{
					force *= 1.5f;
				}
				weapon.RigidbodyItem.velocity += new Vector2(-Mathf.Sign(xDiffWeapons) * force, 0);
			}
		}
	}
}
