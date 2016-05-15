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


	public void SwitchScreen(string sceneName)
	{
		nextSceneName = sceneName;
        transitionObject.FadeIn();
    }

	public void FakeSwitchScreen()
	{
		nextSceneName = "Fake";
        fakeSwitch = true;
        transitionObject.FadeIn();
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
		transitionObject.FadeOut();
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
