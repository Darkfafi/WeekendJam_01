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
			PickUpInfo infoPickUp = objectPicker.PickUpObject<Weapon>(Vector2.up, holderTransform.position, 0.4f, true);
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
		hitBox.HitType = weapon.WeaponHitType;
	}
	public GameObject DropWeapon(bool dropObject = true, DamageHitBox.HitTypes hit = DamageHitBox.HitTypes.Ko)
	{
		GameObject dropWeaponObject = null;
		if (CurrentWeapon != null)
		{
			if (dropObject)
			{
				dropWeaponObject = GameObject.Instantiate(WeaponFactory.GetWeaponGameObject(CurrentWeapon.weapon), holderTransform.position, Quaternion.identity) as GameObject;
			}
			CurrentWeapon = null;
		}
		hitBox.HitType = DamageHitBox.HitTypes.Ko;
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
