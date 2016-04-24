using UnityEngine;
using System.Collections;

public class Corpse : MonoBehaviour {

	public SpriteRenderer SpriteRenderer { get { return corpseSpriteRenderer; } }
	public Player PlayerOwnedCorpse { get; private set; }
	public Player KillerOfCorpse { get; private set; }

	[SerializeField]
	private SpriteRenderer corpseSpriteRenderer;

	public void SetCorpseInfo(Player killed, Player killer)
	{
		PlayerOwnedCorpse = killed;
		KillerOfCorpse = killer;
	}
}
