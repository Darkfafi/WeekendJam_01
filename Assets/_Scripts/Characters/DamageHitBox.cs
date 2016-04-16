using UnityEngine;
using System.Collections;

public class DamageHitBox : MonoBehaviour {

	public delegate void HitBoxHandler(DamageHitBox callerBox, DamageHitBox otherBox);
	public HitBoxHandler HitBoxClashEvent;

	public enum HitTypes
	{
		None,	// No weapon
		Idle,	// Weapon that does not hurt
		Ko,		// Weapon that ko's
		Kill	// Weapon that kills 
	} 

	public HitTypes HitType = HitTypes.None;

	public Character Player { get { return player; } }
	[SerializeField]
	private Character player;


	void OnTriggerEnter2D(Collider2D other)
	{
		DamageHitBox otherHitBox = other.gameObject.GetComponent<DamageHitBox>();
		if(otherHitBox != null)
		{
			if(HitBoxClashEvent != null)
			{
				HitBoxClashEvent(this, otherHitBox);
            }
		}
	}
}
