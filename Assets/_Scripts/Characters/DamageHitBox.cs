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

	// Best way to do this is that the owner is a global entity, then check if the entity is a character and what player it belongs to.
	// But sinse this is a pvp only game this can go through
	public Character Owner { get; private set; } // Character is given to both the thrown weapon and the weapon holder. if the player == null and hit somebody then the kill belongs to that players, else it was a world kill

	public void SetOwner(Character owner)
	{
		Owner = owner;
	}

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
