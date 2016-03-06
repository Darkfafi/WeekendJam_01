using UnityEngine;
using System.Collections;

public class PlayerAnimationHandler {

	private Player player;
	private Animator animator;

	public PlayerAnimationHandler(Player player,Animator animator)
	{
		this.animator = animator;
		this.player = player;
    }

	public void PlayAnimation(string animationString)
	{
		animator.Play(GetAnimationName(animationString));
	}

	public string GetAnimationName(string animationString)
	{
		if ((animationString != "KO" || animationString != "Death"))
		{
			if (player.CurrentWeapon != null)
			{ 
				animationString += player.CurrentWeapon.ItemId;
			}
		}
		return animationString;
	}
}
