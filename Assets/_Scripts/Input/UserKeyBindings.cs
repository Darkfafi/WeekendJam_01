using UnityEngine;
using System.Collections;

public class UserKeyBindings {

	public enum KeyUsers
	{
		Keyboard1,
		Keyboard2,
		JoyStick1,
		JoyStick2
	}


	public static KeyBindings GetKeysOf(KeyUsers user)
	{
		KeyBindings keyBindings = null;
		switch(user)
		{
			case KeyUsers.Keyboard1:
				keyBindings = new KeyBindings(KeyCode.A,KeyCode.D,KeyCode.W,KeyCode.J,KeyCode.K);
                break;
			case KeyUsers.Keyboard2:
				keyBindings = new KeyBindings(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.N, KeyCode.M);
				break;
		}

		return keyBindings;
    }
}
public class KeyBindings
{

	public KeyCode Left { get; private set; }
	public KeyCode Right { get; private set; }
	public KeyCode Jump { get; private set; }
	public KeyCode Attack { get; private set; }
	public KeyCode Use { get; private set; } //Throwing and picking up spear. Also for extra features
	
	public KeyCode[] AllKeys { get; private set; }

	public KeyBindings(KeyCode left, KeyCode right, KeyCode jump, KeyCode attack, KeyCode use)
	{
		this.Left = left;
		this.Right = right;
		this.Jump = jump;
		this.Attack = attack;
		this.Use = use;
		this.AllKeys = new KeyCode[]{ left,right,jump,attack,use};
    }
}
