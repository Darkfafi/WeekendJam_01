using UnityEngine;
using System.Collections;
using Ramses.Confactory;
public class InputUser : MonoBehaviour {

	public delegate void InputHandler(InputAction action, InputUser user);
	public delegate void InputKeyHandler(string name, InputAction.KeyAction keyActionType);
	public delegate void InputAxisHandler(string name, float value);
	public event InputHandler InputEvent;
	public event InputKeyHandler InputKeyEvent;
	public event InputAxisHandler InputAxisEvent;

	public bool InputEnabled { get; private set; }
	public ConGameInputBindings.BindingTypes InputUsing { get { return inputUsing;  } }
	[SerializeField] private ConGameInputBindings.BindingTypes inputUsing;
	private ConInputBindingsHandler inputCon;

	private void Awake()
	{
		inputCon = ConfactoryFinder.Instance.Give<ConInputBindingsHandler>();
    }

	private void OnEnable()
	{
		SetInputEnabled(true);
    }
	private void OnDisable()
	{
		SetInputEnabled(false);
	}

	public void SetInputEnabled(bool enabledState)
	{
		InputEnabled = enabledState;

		if(InputEnabled)
		{
			inputCon.RegisterInputUser(this);
		}
		else
		{
			inputCon.UnRegisterInputUser(this);
		}
    }

	public void SetInputUsing(ConGameInputBindings.BindingTypes bindingType)
	{
		inputUsing = bindingType;
		// TODO Idea: Do the single 0 axis check here (with preAxis and all) so the inputAxisEvent is only called once when the axis hits 0
    }

	// May be called by AI script also. Thats why it is public
	public void OnInput(InputAction inputAction)
	{
		if (InputEnabled)
		{
			if (InputEvent != null)
			{
				InputEvent(inputAction, this);
			}
			if (InputKeyEvent != null && inputAction.Type == InputItem.InputType.KeyCode)
			{
				InputKeyEvent(inputAction.Name, inputAction.KeyActionValue);
			}
			if (InputAxisEvent != null && inputAction.Type == InputItem.InputType.Axis)
			{
				InputAxisEvent(inputAction.Name, inputAction.Value);
			}
		}
	}
}
