using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenTransitionObject : MonoBehaviour {

	public delegate void FadeHandler();
	public event FadeHandler FadeOutCompleteEvent;
	public event FadeHandler FadeInCompleteEvent;
	private float timeMod = 0.015f;
	[SerializeField] private Image block;

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public void FadeIn(float speed = 2.5f)
	{
		StopAllCoroutines();
        Time.timeScale = 0.6f;
		gameObject.SetActive(true);
		StartCoroutine(Animate(1, speed));
	}
	public void FadeOut(float speed = 2.5f)
	{
		StopAllCoroutines();
		gameObject.SetActive(true);
		StartCoroutine(Animate(0, speed));
	}

	private IEnumerator Animate(float opacity, float speed)
	{
		float dir = opacity - block.color.a;
		bool animate = true;
		while (animate)
		{
			block.SetAlpha(block.color.a + (dir * (speed * timeMod)));
			if(dir < 0 && block.color.a <= 0 || dir > 0 && block.color.a >= 1 || dir == 0)
			{
				block.SetAlpha((dir < 0) ? 0 : 1);
				animate = false;
            }
			yield return null;
		}

		if(dir < 0)
		{
			Time.timeScale = 1;
			if (FadeOutCompleteEvent != null)
			{
				FadeOutCompleteEvent();
			}
			gameObject.SetActive(false);
		}
		else
		{
			if(FadeInCompleteEvent != null)
			{
				FadeInCompleteEvent();
            }
		}
	}
}
