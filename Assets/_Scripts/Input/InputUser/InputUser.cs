using UnityEngine;
using System.Collections;
using Confactory;
public class InputUser : MonoBehaviour {

	public delegate void InputHandler(InputAction action, InputUser user);
	public delegate void InputKeyHandler(string name, InputAction.KeyAction keyActionType);
	public delegate void InputAxisHandler(string name, float value);
	public event InputHandler InputEvent;
	public event InputKeyHandler InputKeyEvent;
	public event InputAxisHandler InputAxisEvent;

	public ConGameInputBindings.BindingTypes InputUsing { get { return inputUsing;  } }
	[SerializeField] private ConGameInputBindings.BindingTypes inputUsing;
	private ConInputBindingsHandler inputCon;

	private void Awake()
	{
		inputCon = ConfactoryFinder.Instance.Give<ConInputBindingsHandler>();
		inputCon.RegisterInputUser(this);
    }

	public void SetInputUsing(ConGameInputBindings.BindingTypes bindingType)
	{
		inputUsing = bindingType;
    }

	// May be called by AI script also. Thats why it is public
	public void OnInput(InputAction inputAction)
	{
		if (InputEvent != null)
		{
			InputEvent(inputAction,this);
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

	private void OnDestroy()
	{
		inputCon.UnRegisterInputUser(this);
	}
}
