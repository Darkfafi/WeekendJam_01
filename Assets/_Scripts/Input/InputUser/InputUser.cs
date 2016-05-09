using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	private Dictionary<string, float> itemValueHistories = new Dictionary<string, float>();

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
    }

	public float GetPreviousItemValue(string itemName)
	{
		if (itemValueHistories.ContainsKey(itemName))
		{
			return itemValueHistories[itemName];
		}
		return InputAction.NOT_IN_USE_VALUE; // if not in list then it has not been used yet.
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
			if(!itemValueHistories.ContainsKey(inputAction.Name))
			{
				itemValueHistories.Add(inputAction.Name, 0);
            }
			itemValueHistories[inputAction.Name] = inputAction.Value;
		}
	}
}
