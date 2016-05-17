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
	private int direction = 0;
	private KeyCode keyCode = KeyCode.None;

	public InputItem()
	{

	}
    public InputItem(string axisName, int direction, string nameInputAction)
	{
		SetBinding(axisName, direction, nameInputAction);
    }
	public InputItem(KeyCode keyCode, string nameInputAction)
	{
		SetBinding(keyCode, nameInputAction);
	}

	public void SetBinding(string axisName, int direction, string nameInputAction)
	{
		axisString = axisName;
		keyCode = KeyCode.None;
		Type = InputType.Axis;
		this.direction = direction;
		InputActionName = nameInputAction;
	}
	public void SetBinding(KeyCode keyCode, string nameInputAction)
	{
		this.keyCode = keyCode;
		axisString = string.Empty;
		Type = InputType.KeyCode;

		InputActionName = nameInputAction;
	}

	public float GetUseValue(float lastValueForUser)
	{
		float value = InputAction.NOT_IN_USE_VALUE;
		if (keyCode != KeyCode.None)
		{
			value = KeyCodeCheck();
		}
		else if (!string.IsNullOrEmpty(axisString))
		{
			value = AxisCheck(lastValueForUser);
		}
		return value;
	}

	public InputAction GetInputActionInfo(float lastValueForUser)
	{
		return new InputAction(InputActionName, Type, GetUseValue(lastValueForUser), lastValueForUser);
	}

	private float KeyCodeCheck()
	{
		float value = InputAction.NOT_IN_USE_VALUE;

		if(Input.GetKeyDown(keyCode))
		{
			value = (float)InputAction.KeyAction.OnKeyDown;
		}else if(Input.GetKey(keyCode))
		{
			value = (float)InputAction.KeyAction.KeyDown;
		}else if(Input.GetKeyUp(keyCode))
		{
			value = (float)InputAction.KeyAction.OnKeyUp;
		}

		return value;
	}
	private float AxisCheck(float lastValueForUser)
	{
		float value = InputAction.NOT_IN_USE_VALUE;
		float axisValue = Input.GetAxis(axisString);
        if (Mathf.Abs(axisValue) / axisValue == direction || (lastValueForUser != 0 && axisValue == 0)) // || hits 0 for first time after pre value was in direction then return -1 
		{
			value = Mathf.Abs(axisValue);
        }
		return value;
	}
}
public struct InputAction
{
	public const float NOT_IN_USE_VALUE = 1337;

	public enum KeyAction
	{
		NotUser = (int)NOT_IN_USE_VALUE,
        OnKeyDown = 1,
		KeyDown = 0,
		OnKeyUp = -1
	}

	public string Name { get; private set; }
	public InputItem.InputType Type { get; private set; }
	public float Value { get; private set; } // Keycode == (-1 onReleased, 0 inPress, 1 is onPress), Axis its -1 to 1 axis info
	public float LastValue { get; private set; }

	public InputAction(string name, InputItem.InputType type, float value, float lastValue)
	{
		Name = name;
		Type = type;
		Value = value;
		LastValue = lastValue;
	}
	public KeyAction KeyActionValue
	{
		get {
			if (Value == -1)
			{
				return KeyAction.OnKeyUp;
			}
			else if (Value == 0)
			{
				return KeyAction.KeyDown;
	        }
			else if (Value == 1)
			{
				return KeyAction.OnKeyDown;
			}
			else
			{
				return KeyAction.NotUser;
			}
		}
	}
}
