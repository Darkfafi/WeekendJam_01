using UnityEngine;
using System.Collections;

public class DamageHitBox : MonoBehaviour {

	public delegate void HitBoxHandler(DamageHitBox callerBox, DamageHitBox otherBox);
	public delegate void HitBoxGlobalHandler(DamageHitBox callerBox, Collider2D other);
	public HitBoxHandler HitBoxClashEvent;
	public HitBoxGlobalHandler CollisionEvent;

	public enum HitTypes
	{
		None,	// No weapon
		Idle,	// Weapon that does not hurt
		Ko,		// Weapon that ko's
		Kill	// Weapon that kills 
	} 

	public HitTypes HitType = HitTypes.None;

	void OnTriggerEnter2D(Collider2D other)
	{
		DamageHitBox otherHitBox = other.gameObject.GetComponent<DamageHitBox>();
		if(CollisionEvent != null)
		{
			CollisionEvent(this, other);
        }

		if(otherHitBox != null)
		{
			if(HitBoxClashEvent != null)
			{
				HitBoxClashEvent(this, otherHitBox);
            }
		}
	}
}
