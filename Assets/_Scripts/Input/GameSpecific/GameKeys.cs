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
