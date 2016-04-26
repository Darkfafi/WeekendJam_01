using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAnimationManager : AnimationManager{

	protected Character player { private set; get; }

	public void SetAnimationHandler(Character player, Animator animator)
	{
		SetAnimationHandler(animator);
		this.player = player;
	}

	public override string GetAnimationName(string animationString)
	{
		if ((animationString != "KO" && animationString != "Death"))
		{
			if (player.CurrentWeapon != null)
			{ 
				animationString += player.CurrentWeapon.ItemId;
			}
		}
		return animationString;
	}
}
