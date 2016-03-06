using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {

	public delegate void OnKeyHandler(KeyBindings bindings, KeyCode keyCode);

	public event OnKeyHandler KeyDownEvent;
	public event OnKeyHandler KeyEvent;
	public event OnKeyHandler KeyUpEvent;

	public KeyBindings KeyBindigns { get { return keyBindings; } }
	private KeyBindings keyBindings = UserKeyBindings.GetKeysOf(UserKeyBindings.KeyUsers.Keyboard1);


	public void SetKeyBindings(KeyBindings keybinding)
	{
		this.keyBindings = keybinding;
    }

	public bool AnyBindKeyPressed()
	{
		foreach (KeyCode key in keyBindings.AllKeys)
		{
			if (Input.GetKey(key))
			{
				return true;
			}
		}
		return false;
	}

	void Update () {
		if(keyBindings != null)
		{
			foreach(KeyCode key in keyBindings.AllKeys)
			{
				if (KeyDownEvent != null && Input.GetKeyDown(key))
				{
					KeyDownEvent(keyBindings, key);
                }
				if (KeyEvent != null && Input.GetKey(key))
				{
					KeyEvent(keyBindings, key);
				}
				if (KeyUpEvent != null && Input.GetKeyUp(key))
				{
					KeyUpEvent(keyBindings, key);
				}
			}
		}
		else if(Input.anyKeyDown)
		{
			Debug.LogWarning("User Keybindings not set! Please check all users!");
		}
	}
}
