﻿// Created by | Ramses Di Perna | 09-04-2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterGameLobby : MonoBehaviour {

	[SerializeField] PlayerSlot[] playerSlots;
	[SerializeField] NotificationBar notificationBar;

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
				if (action.Name == InputNames.USE)
				{
					Debug.Log("Back to Menu!(Menu Scene)");
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
					else if(CheckAllSlotsReadyStatus())
					{
						if (AmountOfSlotsInUse() > 1)
						{
							Debug.Log("START(Game Scene)");
						}else
						{
							Debug.Log("Need more then one player to start!");
							notificationBar.ShowNotification("Need more players to start!", 3.5f, 1.2f, 3.5f);
                        }
					}else
					{
						notificationBar.ShowNotification("All Players have to be ready to start!", 3.5f, 1.2f, 3.5f);
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
		playerInfo.playerId = Array.IndexOf(playerSlots, slot);
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
	private int AmountOfSlotsInUse()
	{
		int counter = 0;
		for (int i = 0; i < playerSlots.Length; i++)
		{
			PlayerSlot slot = playerSlots[i];
			if (slot.PlayerOnSlot != null)
			{
				counter++;
            }
			else
			{
				break;
			}
		}
		return counter;
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
	private bool CheckAllSlotsReadyStatus()
	{
		PlayerSlot slot;
		for(int i = 0; i < playerSlots.Length; i++)
		{
			slot = playerSlots[i];
			if (slot.PlayerOnSlot != null)
			{
				if (!slot.IsReady)
				{
					return false;
				}
			}
			else
			{
				if (i == 0)
				{
					return false;
				}
				else
				{
					break;
				}
			}
		}
		return true;
	}
	/// <summary>
	/// Places all players to left slot if possible. This prevents slot 1 being empty while slot 2 is in use.
	/// </summary>
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
		ColorHandler.Colors color = colors[UnityEngine.Random.Range(0, colors.Length)];
		conActivePlayers.ColorHandler.GetColor(color, true);
        return color;
    }
}
