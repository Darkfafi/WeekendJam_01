using UnityEngine;
using System.Collections;

public class PlayerInfo {

	public Color PlayerColor { get; private set; }
	public Character PlayerCharacter { get; private set; }//character of player
	public int PlayerKeyBindingsType { get; private set;} // If null then AI.
	public int playerId = 0;

	public PlayerInfo(int bindingsDefinePlayer)
	{
		PlayerKeyBindingsType = bindingsDefinePlayer;
    }

	public void SetPlayerColor(Color color)
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
