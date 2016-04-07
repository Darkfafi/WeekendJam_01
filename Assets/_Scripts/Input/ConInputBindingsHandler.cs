// Created by | Ramses Di Perna | 06-04-2016

using UnityEngine;
using System.Collections;
using Confactory;
using System;
using System.Collections.Generic;

public class ConInputBindingsHandler : MonoBehaviour, IConfactory {

	//Als de monobehaviour object die de keybindigns inhoud word aangezet "Awake". Dan registreed die zich bij deze confactory, 
	//Bij vernieteging van het object schrijft diezelfde monobehaviour zichzelf weer uit deze confactory
	//De confactory Geeft de informatie van wat er gedrukt is alleen aan degene die ze wilt weten. Dus alleen iemand met Keyboard1 krijgt het te horen als 1 van de key

	private List<InputUser> allInputUsers = new List<InputUser>();
	private ConGameInputBindings gameBindings;
	private GameObject gObject;

    public void ConClear()
	{
		GameObject.Destroy(gObject);
    }

	public void ConStruct()
	{
		//TODO luisterd naar wanneer de Game keybindingsholder confactory bindings krijgt of checkt hier al of het, het al heeft. Dan als er naar gevraagt wordt geeft het geen error. Anders wel
		gameBindings = ConfactoryFinder.Instance.Give<ConGameInputBindings>();
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}

	public void RegisterInputUser(InputUser user)
	{
		if(!allInputUsers.Contains(user))
		{
			allInputUsers.Add(user);
		}
	}
	public void UnRegisterInputUser(InputUser user)
	{
		if (allInputUsers.Contains(user))
		{
			allInputUsers.Remove(user);
		}
	}

	private void Update()
	{
		if(allInputUsers.Count > 0)
		{
			foreach(InputUser user in allInputUsers)
			{
				if (user.InputUsing != ConGameInputBindings.BindingTypes.None)
				{
					InputItem[] allBindingsForUser = gameBindings.GetBindingsOf(user.InputUsing);
					if (allBindingsForUser != null)
					{
						InputAction action;
						foreach (InputItem item in allBindingsForUser)
						{
							action = new InputAction(item.InputActionName, item.Type, item.GetUseValue());
							if (action.Value != InputAction.NOT_IN_USE_VALUE)
							{
								user.OnInput(action);
							}
						}
					}
					else
					{
						Debug.LogError(user.gameObject.name + " asking for bindings for type " + user.InputUsing + " which have not been set yet in ConGameInputBindings!");
					}
				}
            }
		}
	}

}
