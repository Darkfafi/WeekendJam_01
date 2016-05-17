﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Ramses.SectionButtons;
public class LobbyTab : Section{

	public bool OpenState { get; private set; }

	[SerializeField] private Image background; // TODO background image height en width tot de grootte brengen van de displayObjectRect.
	[SerializeField] private RectTransform displayObjectRect;

	private Vector2 startingTabSize;

	// Use this for initialization
	void Awake () {
		startingTabSize = ((RectTransform)transform).sizeDelta;
		displayObjectRect.gameObject.SetActive(false);
		SetActiveState(false);
		background.rectTransform.sizeDelta = startingTabSize;
		background.rectTransform.anchoredPosition = new Vector2(0.5f, 0f); // top center position
    }

	public virtual void GetInput(ConGameInputBindings.BindingTypes type, InputAction action)
	{

	}

	public virtual void Open()
	{
		StopAllCoroutines();
		StartCoroutine(SizeAnimation(background, displayObjectRect.sizeDelta, 10f));
		OpenState = true;
    }

	public virtual void Close()
	{
		StopAllCoroutines();
		StartCoroutine(SizeAnimation(background, startingTabSize, 10f));
		OpenState = false;
    }

	private IEnumerator SizeAnimation(Image animatingObject, Vector2 sizeToAnimateTo, float speed)
	{
		yield return null;
		Vector2 tS = animatingObject.rectTransform.sizeDelta;
		Vector2 diff = sizeToAnimateTo - tS;
		Vector2 dir = diff.normalized;
		Vector2 originalDir = dir;

		bool animating = true;

		if(originalDir.y < 0)
		{
			DisplayObject(Mathf.RoundToInt(originalDir.y));
        }

		while (animating)
		{
			yield return null;
			tS = animatingObject.rectTransform.sizeDelta;
			diff = sizeToAnimateTo - tS;
			dir = diff.normalized;

			if (Mathf.Abs(diff.x) > 0.01f && Mathf.RoundToInt(dir.x) == Mathf.RoundToInt(originalDir.x))
			{
				tS.x += dir.x * speed;
			}
			else if (diff.x != 0)
			{
				tS.x = sizeToAnimateTo.x;
			}

			if (Mathf.Abs(diff.y) > 0.01f && Mathf.RoundToInt(dir.y) == Mathf.RoundToInt(originalDir.y))
			{
				tS.y += dir.y * speed;
			}
			else if (diff.y != 0)
			{
				tS.y = sizeToAnimateTo.y;
			}

			animatingObject.rectTransform.sizeDelta = tS;

			if (animatingObject.rectTransform.sizeDelta == sizeToAnimateTo)
			{
				animating = false;
			}
		}

		if (originalDir.y > 0)
		{
			DisplayObject(Mathf.RoundToInt(originalDir.y));
		}
	}

	private void DisplayObject(int yDirScreen)
	{
		if(yDirScreen == -1)
		{
			displayObjectRect.gameObject.SetActive(false);
		}
		else
		{
			displayObjectRect.gameObject.SetActive(true);
		}
	}
}
