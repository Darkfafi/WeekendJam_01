using UnityEngine;
using System.Collections;
using Confactory;
public class InputUser : MonoBehaviour {

	public delegate void InputHandler(InputAction action);
	public event InputHandler InputEvent; 

	public int InputUsing { get { return inputUsing;  } }
	[SerializeField] private int inputUsing;
	private ConInputBindingsHandler inputCon;

	private void Awake()
	{
		inputCon = ConfactoryFinder.Instance.Give<ConInputBindingsHandler>();
		inputCon.RegisterInputUser(this);
    }

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
