using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIInputSubmitter
{
	private InputUser user;

	private Dictionary<string, float> allInputValues = new Dictionary<string, float>();

	private List<string> usingInputActions = new List<string>();

	public AIInputSubmitter(InputUser user)
	{
		this.user = user;
	}

	public void DoInputAction(string actionName)
	{
		float value = 0;
		value = usingInputActions.Contains(actionName) ? 0 : 1;
		DoInputAction(actionName, value);
    }

	public void ReleaseKey(string actionName)
	{
		if (usingInputActions.Contains(actionName))
		{
			DoInputAction(actionName, -1);
		}
	}

	private void DoInputAction(string actionName, float value)
	{
		if (!allInputValues.ContainsKey(actionName))
		{
			allInputValues.Add(actionName, 0);
		}

		if (!usingInputActions.Contains(actionName) && value != -1)
		{
			usingInputActions.Add(actionName);
		}
		else if(value == -1)
		{
			usingInputActions.Remove(actionName);
		}
		
		user.OnInput(new InputAction(actionName, InputItem.InputType.KeyCode, value, allInputValues[actionName]));
		allInputValues[actionName] = value;
	}
}
