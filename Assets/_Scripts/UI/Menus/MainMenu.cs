using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Ramses.SectionButtons;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private ButtonSectionManager manager;
	private MultiInputUser multiInputUser;
	private float lastAxis = 0;

	private void Awake()
	{
		manager.ButtonPressedEvent += OnButtonPressedEvent;
		multiInputUser = gameObject.GetComponent<MultiInputUser>();
		multiInputUser.InputBindingUsedEvent += OnInputBindingUsedEvent;
    }

	public void OnButtonPressedEvent(ButtonSectionManager manager, ButtonSection section, SectionButton button)
	{
		if (button.ButtonName == "Start")
		{
			SceneManager.LoadScene("Lobby");
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
				manager.CurrentlySelectedSection.PreviousButton();
			}
			else if (action.Name == InputNames.DOWN)
			{
				manager.CurrentlySelectedSection.NextButton();
			}
		}

		if (action.Name == InputNames.UP || action.Name == InputNames.DOWN)
		{
			lastAxis = action.Value;
		}

		if (action.Name == InputNames.ATTACK)
		{
			manager.CurrentlySelectedSection.PressSelectedButton();
		}
	}
}
