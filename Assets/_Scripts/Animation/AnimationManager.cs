using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{

	public delegate void AnimationHandler(string animation);
	public delegate void AnimationFloatHandler(string animation, float finishNormTime);

	public event AnimationHandler AnimationStartedEvent;
	public event AnimationFloatHandler AnimationEndedEvent;

	protected Animator animator { private set; get; }

	public int AnimationLoopCounter { get; private set; }
	public float AnimationNormalizedTime { get; private set; }

	private Dictionary<string, string> GivenAndActiveAnimationNames = new Dictionary<string, string>();

	public string AnimationPlaying
	{
		get { return animationPlaying; }
		set
		{
			string old = animationPlaying;
			if (!string.IsNullOrEmpty(animationPlaying) && value != animationPlaying)
			{
				float finishedTime = (AnimationNormalizedTime > 1) ? 1 : AnimationNormalizedTime; // if filled with an empty string then the animation has ended by completion
				AnimationEnd(animationPlaying, finishedTime);
			}
			animationPlaying = value;
			if (!string.IsNullOrEmpty(animationPlaying) && old != animationPlaying)
			{
				AnimationStart(animationPlaying);
			}
		}
	}
	private string animationPlaying = "";

	public virtual void SetAnimationHandler(Animator animator)
	{
		this.animator = animator;
	}

	public virtual void PlayAnimation(string animationString)
	{
		string realNameAnim = GetAnimationName(animationString);
        animator.Play(realNameAnim);
		AnimationPlaying = realNameAnim;
		if (!GivenAndActiveAnimationNames.ContainsKey(realNameAnim))
		{
			GivenAndActiveAnimationNames.Add(realNameAnim, animationString);
		}
	}

	public virtual string GetAnimationName(string animString)
	{
		return animString;
	}

	/// <summary>
	/// Returns if any of the converted versions of the baseOfCheckName is in play or around end of playing.
	/// if checkIfPlayedAnimation == true it will only look if the specific animationNameToCheck has been played and is a version of baseOfCheckName.
	/// Example: animationNameToCheck == 'AttackSpear'. baseOfCheckName == 'Attack'. if AttackSpear is playing then this will return true.
	/// if 'AttackGun' is playing and checkIfPlayedAnimation == true then it will return false. Else it will return true because its a converted version of 'Attack'
	/// </summary>
	/// <param name="animationString"></param>
	/// <param name="compairAnimationName"></param>
	/// <returns></returns>
	public virtual bool PlayedAnimationNameCheck(string animationNameToCheck, string baseOfCheckName, bool checkIfPlayedAnimation = false)
	{
		bool isEqual = animationNameToCheck == baseOfCheckName;
		if(!isEqual)
		{
			if (checkIfPlayedAnimation)
			{
				isEqual = GivenAndActiveAnimationNames.ContainsValue(baseOfCheckName) && GivenAndActiveAnimationNames.ContainsKey(animationNameToCheck);
			}
			else
			{
				isEqual = GivenAndActiveAnimationNames.ContainsValue(baseOfCheckName);
            }
        }
		return isEqual;
	}

	public bool AnimatorInAnimation(string name)
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName(GetAnimationName(name));
    }

	protected virtual void LateUpdate()
	{
		if (!string.IsNullOrEmpty(AnimationPlaying))
		{
			AnimationNormalizedTime = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime - AnimationLoopCounter) / 1;
			if (AnimationNormalizedTime >= 1)
			{
				AnimationNormalizedTime = 1;
				AnimationLoopCounter++;
            }
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationPlaying) || (AnimationNormalizedTime == 1 && !animator.GetCurrentAnimatorStateInfo(0).loop))
			{
				AnimationPlaying = "";
			}
		}
	}
	protected virtual void AnimationStart(string animationName)
	{
		AnimationLoopCounter = 0;
		AnimationNormalizedTime = 0;
        if (AnimationStartedEvent != null)
		{
			AnimationStartedEvent(animationName);
		}
	}
	protected void AnimationEnd(string animationName, float finishedTime)
	{
		if (AnimationEndedEvent != null)
		{
			AnimationEndedEvent(animationName, finishedTime);
		}
		if (GivenAndActiveAnimationNames.ContainsKey(animationName))
		{
			GivenAndActiveAnimationNames.Remove(animationName);
		}
	}
}
