using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Ramses.SectionButtons;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private SectionHolder holder;
	private MultiInputUser multiInputUser;
	private float lastAxis = 0;

	private void Awake()
	{
		holder.ButtonPressedEvent += OnButtonPressedEvent;
		multiInputUser = gameObject.GetComponent<MultiInputUser>();
		multiInputUser.InputBindingUsedEvent += OnInputBindingUsedEvent;
    }

	public void OnButtonPressedEvent(SectionHolder holder, Section section, SectionButton button)
	{
		if (button.ButtonName == "Start")
		{
			Ramses.Confactory.ConfactoryFinder.Instance.Give<ConSceneSwitcher>().SwitchScreen("Lobby");
		}
		if (button.ButtonName == "Quit")
		{
			Application.Quit();
		}
	}

	private void OnInputBindingUsedEvent(ConGameInputBindings.BindingTypes type, InputAction action)
	{
		if ((lastAxis == 0 && action.Type == InputItem.InputType.Axis)||
			(action.Type == InputItem.InputType.KeyCode && action.KeyActionValue == InputAction.KeyAction.OnKeyDown))
		{
			if (action.Name == InputNames.UP)
			{
				holder.CurrentSection.PreviousButton();
			}
			else if (action.Name == InputNames.DOWN)
			{
				holder.CurrentSection.NextButton();
			}
		}

		if (action.Name == InputNames.UP || action.Name == InputNames.DOWN)
		{
			lastAxis = action.Value;
		}

		if (action.Name == InputNames.ATTACK)
		{
			holder.CurrentSection.CurrentButton.Press();
		}
	}
}
