﻿using UnityEngine;
using System.Collections;

public class PlayerInfo {

	public ColorHandler.Colors PlayerColor { get; private set; }
	public Character PlayerCharacter { get; private set; }//character of player
	public ConGameInputBindings.BindingTypes BindingType { get; private set;} // If null then AI.
	public int playerId = 0;

	public PlayerInfo(ConGameInputBindings.BindingTypes bindingsDefinePlayer)
	{
		BindingType = bindingsDefinePlayer;
    }

	public void SetPlayerColor(ColorHandler.Colors color)
	{
		PlayerColor = color;
		//Send event that color changed;
    }
	public void SetPlayerId(int id)
	{
		playerId = id;
    }
	public void SetCharacter(Character character)
	{
		PlayerCharacter = character;
		// Send event that character has been set
		// TODO: In the spawn class it looks at all the active players, creates a character for each of them and sets it in their data.
    }
}
