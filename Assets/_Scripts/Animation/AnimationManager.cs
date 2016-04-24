using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{

	public delegate void AnimationHandler(string animation);
	public delegate void AnimationFloatHandler(string animation, float finishNormTime);

	public event AnimationHandler AnimationStarted;
	public event AnimationFloatHandler AnimationEnded;

	protected Character player { private set; get; }
	protected Animator animator { private set; get; }

	public int AnimationLoopCounter { get; private set; }
	public float AnimationNormalizedTime { get; private set; }

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

	public virtual void SetAnimationHandler(Character player, Animator animator)
	{
		this.animator = animator;
		this.player = player;
	}

	public virtual void PlayAnimation(string animationString)
	{
		string realNameAnim = GetAnimationName(animationString);
        animator.Play(realNameAnim);
		AnimationPlaying = realNameAnim;
	}

	public virtual string GetAnimationName(string animString)
	{
		return animString;
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
        if (AnimationStarted != null)
		{
			AnimationStarted(animationName);
		}
	}
	protected void AnimationEnd(string animationName, float finishedTime)
	{
		if (AnimationEnded != null)
		{
			AnimationEnded(animationName, finishedTime);
		}
	}
}
