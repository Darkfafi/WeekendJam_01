using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Ramses.SectionButtons;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private ButtonSectionManager manager;

	private MultiInputUser multiInputUser;
	private Dictionary<ConGameInputBindings.BindingTypes, float> axisUseValue = new Dictionary<ConGameInputBindings.BindingTypes, float>();

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
		if(button.ButtonName == "Quit")
		{
			Application.Quit();
		}
	}

	private void OnInputBindingUsedEvent(ConGameInputBindings.BindingTypes type, InputAction action)
	{
		if (action.Type == InputItem.InputType.Axis)
		{
			if (!axisUseValue.ContainsKey(type))
			{
				axisUseValue.Add(type, 0);
            }
		}

		if ((action.KeyActionValue == InputAction.KeyAction.OnKeyDown && action.Type == InputItem.InputType.KeyCode)
			|| (action.Type == InputItem.InputType.Axis && axisUseValue[type] == 0)) {
			if (action.Name == InputNames.UP)
			{
				manager.CurrentlySelectedSection.PreviousButton();
			}
			else if (action.Name == InputNames.DOWN)
			{
				manager.CurrentlySelectedSection.NextButton();
			}
		}else if(action.Name == InputNames.ATTACK || action.Name == InputNames.JUMP)
		{
			manager.CurrentlySelectedSection.PressSelectedButton();
		}
		if (action.Type == InputItem.InputType.Axis && (action.Name == InputNames.UP || action.Name == InputNames.DOWN))
		{
			if (axisUseValue[type] == 0)
			{
				axisUseValue[type] = action.Value * ((action.Name == InputNames.UP) ? -1 : 1);
            }
			else
			{
				if(axisUseValue[type] < 0 && action.Name == InputNames.UP)
				{
					axisUseValue[type] = action.Value;
                }
				else if(action.Name == InputNames.DOWN && axisUseValue[type] > 0)
				{
					axisUseValue[type] = action.Value;
				}
			}
		}
	}
}
