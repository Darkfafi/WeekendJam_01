using UnityEngine;
using System.Collections;
using Confactory;
public class GameKeys : MonoBehaviour {

	public enum KeyBindingTypes
	{
		Keyboard01,
		Keyboard02,
		Joystick01,
		Joystick02
	}

	private ConGameInputBindings bindingsSystem;

	void Awake()
	{
		bindingsSystem = ConfactoryFinder.Instance.Give<ConGameInputBindings>();
		bindingsSystem.AddBindings(0, new InputItem[]
		{
			new InputItem(KeyCode.A,InputNames.LEFT),
			new InputItem(KeyCode.D,InputNames.RIGHT),
			new InputItem(KeyCode.W,InputNames.JUMP),
			new InputItem(KeyCode.J,InputNames.ATTACK),
			new InputItem(KeyCode.K,InputNames.USE)
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
