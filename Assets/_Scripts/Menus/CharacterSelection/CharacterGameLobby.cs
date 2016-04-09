// Created by | Ramses Di Perna | 09-04-2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CharacterGameLobby : MonoBehaviour {

	[SerializeField] PlayerSlot[] playerSlots;
	private MultiInputUser multiInputUser;
	private ConActivePlayers conActivePlayers;

	void Awake ()
	{
		conActivePlayers = Confactory.ConfactoryFinder.Instance.Give<ConActivePlayers>();
		multiInputUser = gameObject.GetComponent<MultiInputUser>();
		multiInputUser.InputBindingUsedEvent += OnInputBindingUsedEvent;
    }
	
	void OnInputBindingUsedEvent(ConGameInputBindings.BindingTypes type, InputAction action)
	{
		if (action.KeyActionValue == InputAction.KeyAction.OnKeyDown) {
			PlayerSlot slot = CheckIfTypeInSlot(type);
			
			if (slot == null)
			{
				if (action.Name == InputNames.ATTACK)
				{
					//add to slot
					PlayerInfo player = new PlayerInfo(type);
					player.SetPlayerColor(GivePlayerRandomColor());
					if (AddPlayerToNextOpenSlot(player))
					{
						conActivePlayers.RegisterPlayer(player);
                    }
				}
			}
			else
			{
				if (action.Name == InputNames.ATTACK)
				{
					//ready in slot
					if (!slot.IsReady)
					{
						slot.SetReady(true);
					}
				}
				if (action.Name == InputNames.USE)
				{
					// Unready or Remove from slot
					if (slot.IsReady)
					{
						slot.SetReady(false);
					}
					else
					{
						RemovePlayerFromSlot(slot);
                    }
				}
			}
			
		}
	}

	public bool AddPlayerToNextOpenSlot(PlayerInfo playerInfo)
	{
		for(int i = 0; i < playerSlots.Length; i++)
		{
			PlayerSlot slot = playerSlots[i];
            if (slot.PlayerOnSlot == null)
			{
				SetSlotInUse(slot, playerInfo);
                return true;
			}
		}
		return false;
	}
	public void RemovePlayerFromSlot(PlayerInfo playerInfo,bool permanent = true)
	{
		for (int i = 0; i < playerSlots.Length; i++)
		{
			PlayerSlot slot = playerSlots[i];
			if (slot.PlayerOnSlot == playerInfo)
			{
				RemovePlayerFromSlot(slot, permanent);
				break;
            }
		}
	}
	public void RemovePlayerFromSlot(PlayerSlot playerSlot, bool permanent = true)
	{
		if(permanent)
		{
			conActivePlayers.UnRegisterPlayer(playerSlot.PlayerOnSlot);
		}
		playerSlot.SetPlayerForSlot(null);
		RefreshPlayerSlots();
	}

	private void SetSlotInUse(PlayerSlot slot, PlayerInfo playerInfo)
	{
		slot.SetPlayerForSlot(playerInfo);
		//TODO sound effect in confactory for sounds or something..
	}
	private PlayerSlot CheckIfTypeInSlot(ConGameInputBindings.BindingTypes type)
	{
		for(int i = 0; i < playerSlots.Length; i++)
		{
			PlayerSlot slot = playerSlots[i];
			if(slot.PlayerOnSlot != null)
			{
				if(slot.PlayerOnSlot.BindingType == type)
				{
					return slot;
				}
			}
		}
		return null;
	}
	private bool AllSlotsInUse()
	{
		for (int i = 0; i < playerSlots.Length; i++)
		{
			PlayerSlot slot = playerSlots[i];
			if (slot.PlayerOnSlot == null)
			{
				return false;
			}
		}
		return true;
	}
	private void RefreshPlayerSlots()
	{
		PlayerSlot currentSlot;
		PlayerSlot nextSlot;
		PlayerInfo nextPlayer;
		bool readyStatus = false;
		for(int i = 0; i < playerSlots.Length - 1; i++)
		{
			currentSlot = playerSlots[i];
			if (currentSlot.PlayerOnSlot == null)
			{
				nextSlot = playerSlots[i + 1];
				if(nextSlot.PlayerOnSlot != null)
				{
					readyStatus = nextSlot.IsReady;
					nextPlayer = nextSlot.PlayerOnSlot;
					RemovePlayerFromSlot(nextSlot,false);
					SetSlotInUse(currentSlot, nextPlayer);
					currentSlot.SetReady(readyStatus);
                }
			}
        }
	}
	private ColorHandler.Colors GivePlayerRandomColor()
	{
		ColorHandler.Colors[] colors = conActivePlayers.ColorHandler.GetAvailableColors();
		ColorHandler.Colors color = colors[Random.Range(0, colors.Length)];
		conActivePlayers.ColorHandler.GetColor(color, true);
        return color;
    }
}
