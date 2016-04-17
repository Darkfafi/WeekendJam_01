using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIPlayerInfo : MonoBehaviour {

	public Player LinkedPlayer { get; private set; }

	[SerializeField]
	private Image backgroundImage;

	public void SetLinkedPlayer(Player player)
	{
		LinkedPlayer = player;
		backgroundImage.color = ColorHandler.ColorsToColor(player.PlayerColor);
    }
}
