using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{

	public delegate void AnimationHandler(string animation);

	public event AnimationHandler AnimationStarted;
	public event AnimationHandler AnimationEnded;

	protected Character player { private set; get; }
	protected Animator animator { private set; get; }

	public string AnimationPlaying
	{
		get { return animationPlaying; }
		set
		{
			string old = animationPlaying;
			if (!string.IsNullOrEmpty(animationPlaying) && value != animationPlaying)
			{
				AnimationEnded(animationPlaying);
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

	protected virtual void LateUpdate()
	{
		if (!string.IsNullOrEmpty(AnimationPlaying))
		{
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationPlaying))
			{
				AnimationPlaying = "";
			}
		}
	}
	protected virtual void AnimationStart(string animationName)
	{
		if (AnimationStarted != null)
		{
			AnimationStarted(animationName);
		}
	}
	protected void AnimationEnd(string animationName)
	{
		if (AnimationEnded != null)
		{
			AnimationEnded(animationName);
		}
	}
}
