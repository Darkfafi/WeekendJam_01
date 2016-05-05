using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiInputUser : MonoBehaviour {

	public delegate void BindingHandler(ConGameInputBindings.BindingTypes type, InputAction action);
	public event BindingHandler InputBindingUsedEvent;

	[SerializeField]
	private List<ConGameInputBindings.BindingTypes> allBindingTypes = new List<ConGameInputBindings.BindingTypes>();

	private Dictionary<ConGameInputBindings.BindingTypes,InputUser> allInputUsers = new Dictionary<ConGameInputBindings.BindingTypes, InputUser>(); 

	void Awake()
	{
		if (allBindingTypes.Count > 0)
		{
			foreach (ConGameInputBindings.BindingTypes bindingType in allBindingTypes)
			{
				if (!allInputUsers.ContainsKey(bindingType))
				{
					CreateInputUser(bindingType);
				}
			}
		}
	}

	private void OnInputEvent(InputAction action, InputUser user)
	{
		if (InputBindingUsedEvent != null)
		{
			InputBindingUsedEvent(user.InputUsing, action);
        }
	}

	public void AddBindingType(ConGameInputBindings.BindingTypes bindingType)
	{
		if (!allBindingTypes.Contains(bindingType))
		{
			allBindingTypes.Add(bindingType);
			CreateInputUser(bindingType);
		}
	}
	public void RemoveBindingType(ConGameInputBindings.BindingTypes bindingType)
	{
		if (allInputUsers.ContainsKey(bindingType))
		{
			InputUser user = allInputUsers[bindingType];
			user.InputEvent -= OnInputEvent;
            allBindingTypes.Remove(bindingType);
			allInputUsers.Remove(bindingType);
			Destroy(user);
		}
	}

	private void CreateInputUser(ConGameInputBindings.BindingTypes bindingType)
	{
		if (bindingType != ConGameInputBindings.BindingTypes.None)
		{
			InputUser user = gameObject.AddComponent<InputUser>();
			allInputUsers.Add(bindingType, user);
			user.SetInputUsing(bindingType);
			user.InputEvent += OnInputEvent;
		}
	}
	void OnDestroy()
	{
		foreach(KeyValuePair<ConGameInputBindings.BindingTypes, InputUser> pair in allInputUsers)
		{
			pair.Value.InputEvent -= OnInputEvent;
			Destroy(pair.Value);
        }
		allBindingTypes = new List<ConGameInputBindings.BindingTypes>();
		allInputUsers = new Dictionary<ConGameInputBindings.BindingTypes, InputUser>();
    }

}
