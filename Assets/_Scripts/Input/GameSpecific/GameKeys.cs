using UnityEngine;
using System.Collections;
using Confactory;
public class GameKeys : MonoBehaviour {

	private ConGameInputBindings bindingsSystem;

	void Awake()
	{
		bindingsSystem = ConfactoryFinder.Instance.Give<ConGameInputBindings>();

		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Keyboard01, new InputItem[]
		{
			new InputItem(KeyCode.A,InputNames.LEFT),
			new InputItem(KeyCode.D,InputNames.RIGHT),
			new InputItem(KeyCode.W,InputNames.JUMP),
			new InputItem(KeyCode.J,InputNames.ATTACK),
			new InputItem(KeyCode.K,InputNames.USE)
		});

		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Keyboard02, new InputItem[]
		{
			new InputItem(KeyCode.LeftArrow,InputNames.LEFT),
			new InputItem(KeyCode.RightArrow,InputNames.RIGHT),
			new InputItem(KeyCode.UpArrow,InputNames.JUMP),
			new InputItem(KeyCode.N,InputNames.ATTACK),
			new InputItem(KeyCode.M,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick01, new InputItem[]
		{
			new InputItem("Horizontal",-1,InputNames.LEFT),
			new InputItem("Horizontal",1,InputNames.RIGHT),
			new InputItem(KeyCode.Joystick1Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick1Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick1Button2,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick02, new InputItem[]
		{
			new InputItem("Horizontal2",-1,InputNames.LEFT),
			new InputItem("Horizontal2",1,InputNames.RIGHT),
			new InputItem(KeyCode.Joystick2Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick2Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick2Button2,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick03, new InputItem[]
		{
			new InputItem("Horizontal3",-1,InputNames.LEFT),
			new InputItem("Horizontal3",1,InputNames.RIGHT),
			new InputItem(KeyCode.Joystick3Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick3Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick3Button2,InputNames.USE)
		});
		bindingsSystem.AddBindings(ConGameInputBindings.BindingTypes.Joystick04, new InputItem[]
		{
			new InputItem("Horizontal4",-1,InputNames.LEFT),
			new InputItem("Horizontal4",1,InputNames.RIGHT),
			new InputItem(KeyCode.Joystick4Button1,InputNames.JUMP),
			new InputItem(KeyCode.Joystick4Button0,InputNames.ATTACK),
			new InputItem(KeyCode.Joystick4Button2,InputNames.USE)
		});
	}
}

public static class InputNames
{
	public const string LEFT = "Left";
	public const string RIGHT = "Right";
	public const string JUMP = "Jump";
	public const string ATTACK = "Attack";
	public const string USE = "Use";
}
