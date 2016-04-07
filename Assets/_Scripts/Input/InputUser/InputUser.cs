using UnityEngine;
using System.Collections;
using Confactory;
public class InputUser : MonoBehaviour {

	public delegate void InputHandler(InputAction action);
	public event InputHandler InputEvent; 

	public ConGameInputBindings.BindingTypes InputUsing { get { return inputUsing;  } }
	[SerializeField] private ConGameInputBindings.BindingTypes inputUsing;
	private ConInputBindingsHandler inputCon;

	private void Awake()
	{
		inputCon = ConfactoryFinder.Instance.Give<ConInputBindingsHandler>();
		inputCon.RegisterInputUser(this);
		if(InputUsing == ConGameInputBindings.BindingTypes.None)
		{
			Debug.LogWarning(gameObject.name + " has no input type assigned!");
		}
    }

	// May be called by AI script also. Thats why it is public
	public void OnInput(InputAction inputAction)
	{
		if(InputEvent != null)
		{
			InputEvent(inputAction);
        }
    }

	private void OnDestroy()
	{
		inputCon.UnRegisterInputUser(this);
	}
}
