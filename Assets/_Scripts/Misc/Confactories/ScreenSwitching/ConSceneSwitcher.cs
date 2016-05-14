using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;
using UnityEngine.SceneManagement;

public class ConSceneSwitcher : IConfactory
{
	ScreenTransitionObject transitionObject;
	private string nextSceneName = "NO_SCENE";
	private bool fakeSwitch = false;
	private float fadeOutSpeed = 2.5f;


	public void SwitchScreen(string sceneName, float fadeInSpeed = 2.5f, float fadeOutSpeed = 2.5f)
	{
		nextSceneName = sceneName;
		this.fadeOutSpeed = fadeOutSpeed;
        transitionObject.FadeIn(fadeInSpeed);
    }

	public void FakeSwitchScreen(float fadeInSpeed = 2.5f, float fadeOutSpeed = 2.5f)
	{
		nextSceneName = "Fake";
		this.fadeOutSpeed = fadeOutSpeed;
        fakeSwitch = true;
        transitionObject.FadeIn(fadeInSpeed);
	}

	public void ConClear()
	{
		transitionObject.FadeInCompleteEvent -= FadeInComplete;
		GameObject.Destroy(transitionObject.gameObject);
	}

	public void ConStruct()
	{
		transitionObject = GameObject.Instantiate<ScreenTransitionObject>(Resources.Load<ScreenTransitionObject>("UI/TransitionScreen"));
		ConfactoryTools.SetObjectWithConGameObjectSettings(transitionObject.gameObject);
		transitionObject.FadeInCompleteEvent -= FadeInComplete;
		transitionObject.FadeInCompleteEvent += FadeInComplete;
    }

	public void OnSceneSwitch(int newSceneIndex)
	{
		transitionObject.FadeOut(fadeOutSpeed);
	}

	private void FadeInComplete()
	{
		if (!fakeSwitch)
		{
			SceneManager.LoadScene(nextSceneName);
		}else
		{
			fakeSwitch = false;
			OnSceneSwitch(0);
        }
		nextSceneName = "NO_SCENE";
	}
}
