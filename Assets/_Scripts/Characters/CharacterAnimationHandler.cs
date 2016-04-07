using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAnimationManager : AnimationManager{

	public override string GetAnimationName(string animationString)
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
