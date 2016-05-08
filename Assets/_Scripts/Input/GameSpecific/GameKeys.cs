﻿using UnityEngine;
using System.Collections;
using Ramses.Confactory;
public class GameKeys : MonoBehaviour {

	private ConGameInputBindings bindingsSystem;

	void Awake()
	{
		bindingsSystem = ConfactoryFinder.Instance.Give<ConGameInputBindings>();

		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Keyboard01, new InputItem[]
		{
			new InputItem(KeyCode.A,InputNames.LEFT),
			new InputItem(KeyCode.D,InputNames.RIGHT),
			new InputItem(KeyCode.W,InputNames.UP),
			new InputItem(KeyCode.S,InputNames.DOWN),
			new InputItem(KeyCode.W,InputNames.JUMP),
			new InputItem(KeyCode.J,InputNames.ATTACK),
			new InputItem(KeyCode.K,InputNames.GRAB_THROW),
			new InputItem(KeyCode.L,InputNames.USE),
		});

		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Keyboard02, new InputItem[]
		{
			new InputItem(KeyCode.LeftArrow,InputNames.LEFT),
			new InputItem(KeyCode.RightArrow,InputNames.RIGHT),
			new InputItem(KeyCode.UpArrow,InputNames.UP),
			new InputItem(KeyCode.DownArrow,InputNames.DOWN),
			new InputItem(KeyCode.UpArrow,InputNames.JUMP),
			new InputItem(KeyCode.B,InputNames.ATTACK),
			new InputItem(KeyCode.N,InputNames.GRAB_THROW),
			new InputItem(KeyCode.M,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick01, new InputItem[]
		{
			new InputItem("Horizontal",-1,InputNames.LEFT),
			new InputItem("Horizontal",1,InputNames.RIGHT),
			new InputItem("Vertical1",-1,InputNames.UP),
			new InputItem("Vertical1",1,InputNames.DOWN),
			new InputItem(KeyCode.Joystick1Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick1Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick1Button2,InputNames.GRAB_THROW),
			new InputItem(KeyCode.Joystick1Button3,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick02, new InputItem[]
		{
			new InputItem("Horizontal2",-1,InputNames.LEFT),
			new InputItem("Horizontal2",1,InputNames.RIGHT),
			new InputItem("Vertical2",-1,InputNames.UP),
			new InputItem("Vertical2",1,InputNames.DOWN),
			new InputItem(KeyCode.Joystick2Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick2Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick2Button2,InputNames.GRAB_THROW),
			new InputItem(KeyCode.Joystick2Button3,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick03, new InputItem[]
		{
			new InputItem("Horizontal3",-1,InputNames.LEFT),
			new InputItem("Horizontal3",1,InputNames.RIGHT),
			new InputItem("Vertical3",-1,InputNames.UP),
			new InputItem("Vertical3",1,InputNames.DOWN),
			new InputItem(KeyCode.Joystick3Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick3Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick3Button2,InputNames.GRAB_THROW),
			new InputItem(KeyCode.Joystick3Button3,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick04, new InputItem[]
		{
			new InputItem("Horizontal4",-1,InputNames.LEFT),
			new InputItem("Horizontal4",1,InputNames.RIGHT),
			new InputItem("Vertical4",-1,InputNames.UP),
			new InputItem("Vertical4",1,InputNames.DOWN),
			new InputItem(KeyCode.Joystick4Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick4Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick4Button2,InputNames.GRAB_THROW),
			new InputItem(KeyCode.Joystick4Button3,InputNames.USE)
		});
	}
}

public static class InputNames
{
	public const string LEFT = "Left";
	public const string RIGHT = "Right";
	public const string UP = "Up";
	public const string DOWN = "Down";
	public const string JUMP = "Jump";
	public const string ATTACK = "Attack";
	public const string GRAB_THROW = "Grab/Throw";
	public const string USE = "Use";
}
