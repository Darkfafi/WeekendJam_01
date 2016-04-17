using UnityEngine;
using System.Collections;

public class Corpse : MonoBehaviour {

	public SpriteRenderer SpriteRenderer { get { return corpseSpriteRenderer; } }
	public Player playerOwnedCorpse;
	public Player killerOfCorpse;

	[SerializeField]
	private SpriteRenderer corpseSpriteRenderer;
}
