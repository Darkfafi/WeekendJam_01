using UnityEngine;
using System.Collections;

public class DamageHitBox : MonoBehaviour {

	public delegate void HitBoxHandler(DamageHitBox callerBox, DamageHitBox otherBox);
	public HitBoxHandler HitBoxClashEvent;

	public enum HitTypes
	{
		None,
		Ko,
		Kill
	}

	public HitTypes HitType = HitTypes.None;

	public Player Player { get { return player; } }
	[SerializeField]
	private Player player;


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
