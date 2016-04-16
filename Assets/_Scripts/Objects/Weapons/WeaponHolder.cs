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
			hitBox.HitType = DamageHitBox.HitTypes.Ko;
		}
		hitBox.gameObject.SetActive(true);
	}

	public GameObject DropWeapon(bool dropObject = true, Vector3? spawnPosition = null)
	{
		GameObject dropWeaponObject = null;
		if (CurrentWeapon != null)
		{
			if (dropObject)
			{
				Vector3 spawnPos = (spawnPosition.HasValue) ? spawnPosition.Value : holderTransform.position;
				dropWeaponObject = GameObject.Instantiate(WeaponFactory.GetWeaponGameObject(CurrentWeapon.weapon), spawnPos, Quaternion.identity) as GameObject;
			}
			CurrentWeapon = null;
		}
		SetWeaponHitbox(false);
        return dropWeaponObject;
    }

	private void OnHitBoxClashEvent(DamageHitBox ownBox, DamageHitBox otherBox)
	{
		if (ownBox.HitType == otherBox.HitType)
		{
			Debug.Log("Clash");
		}
		else if(ownBox.HitType == DamageHitBox.HitTypes.Kill && otherBox.HitType == DamageHitBox.HitTypes.Ko)
		{
			GameObject weapon = DropWeapon(true);
		}
	}
}
