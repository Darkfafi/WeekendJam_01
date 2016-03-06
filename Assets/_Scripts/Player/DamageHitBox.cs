using UnityEngine;
using System.Collections;

public class DamageHitBox : MonoBehaviour {

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
}
