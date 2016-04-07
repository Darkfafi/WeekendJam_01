// Created by | Ramses Di Perna | 06-04-2016

using UnityEngine;
using System.Collections;

public class InputItem {

	public enum InputType
	{
		Axis,
		KeyCode
	}
	public InputType Type { get; private set; }
	public string InputActionName { get; private set; }

	private string axisString = string.Empty;
	private KeyCode keyCode = KeyCode.None;

	public InputItem()
	{

	}
    public InputItem(string axisName, string nameInputAction)
	{
		SetBinding(axisName, nameInputAction);
    }
	public InputItem(KeyCode keyCode, string nameInputAction)
	{
		SetBinding(keyCode, nameInputAction);
	}

	public void SetBinding(string axisName, string nameInputAction)
	{
		axisString = axisName;
		keyCode = KeyCode.None;
		Type = InputType.Axis;

		InputActionName = nameInputAction;
	}
	public void SetBinding(KeyCode keyCode, string nameInputAction)
	{
		this.keyCode = keyCode;
		axisString = string.Empty;
		Type = InputType.KeyCode;

		InputActionName = nameInputAction;
	}

	public float GetUseValue()
	{
		float value = InputAction.NOT_IN_USE_VALUE;
		if (keyCode != KeyCode.None)
		{
			value = KeyCodeCheck();
		}
		else if (!string.IsNullOrEmpty(axisString))
		{
			value = AxisCheck();
		}
		return value;
	}

	public InputAction GetInputActionInfo()
	{
		float value = InputAction.NOT_IN_USE_VALUE;
		if (keyCode != KeyCode.None)
		{
			value = KeyCodeCheck();
        }
		else if(!string.IsNullOrEmpty(axisString))
		{
			value = AxisCheck();
        }
		return new InputAction(InputActionName, Type, value);
	}

	private float KeyCodeCheck()
	{
		float value = InputAction.NOT_IN_USE_VALUE;

		if(Input.GetKeyDown(keyCode))
		{
			value = 1;
		}else if(Input.GetKey(keyCode))
		{
			value = 0;
		}else if(Input.GetKeyUp(keyCode))
		{
			value = -1;
		}

		return value;
	}
	private float AxisCheck()
	{
		float value = Input.GetAxis(axisString);
		return value;
	}
}
public class InputAction
{
	public const float NOT_IN_USE_VALUE = 1337;
	public const int VALUE_ON_KEY_DOWN = 1;
	public const int VALUE_KEY_DOWN = 0;
	public const int VALUE_ON_KEY_UP = -1;


	public string Name { get; private set; }
	public InputItem.InputType Type { get; private set; }
	public float Value { get; private set; } // Keycode == (-1 onReleased, 0 inPress, 1 is onPress), Axis its -1 to 1 axis info

	public InputAction(string name, InputItem.InputType type, float value)
	{
		Name = name;
		Type = type;
		Value = value;
	}
}
