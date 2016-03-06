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
    }

	public void PickUpWeapon(bool dropHoldingWeapon = true)
	{
			PickUpInfo infoPickUp = objectPicker.PickUpObject<Weapon>(Vector2.down, holderTransform.position, 0.1f, true);
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
		if (weapon.KillWeapon)
		{
			hitBox.HitType = DamageHitBox.HitTypes.Kill;
		}
		else
		{
			hitBox.HitType = DamageHitBox.HitTypes.Ko;
		}
	}
	public void DropWeapon(bool dropObject = true)
	{
		if (CurrentWeapon != null)
		{
			if (dropObject)
			{
				GameObject.Instantiate(WeaponFactory.GetWeaponGameObject(CurrentWeapon.weapon), holderTransform.position, Quaternion.identity);
			}
			hitBox.HitType = DamageHitBox.HitTypes.Ko;
			CurrentWeapon = null;
		}
	}
}
