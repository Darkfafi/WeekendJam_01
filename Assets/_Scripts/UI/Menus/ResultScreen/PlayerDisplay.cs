using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour {

	// Display itself
	[SerializeField] private Text rankText;
	[SerializeField] private Image backgroundImage;
	[SerializeField] private Image flagImage;

	// Game Rules Specifics
	public StocksEndResultDisplay StocksEndResultDisplay { get { return stocksEndResultDisplay; } }
	[SerializeField] private StocksEndResultDisplay stocksEndResultDisplay;

	// other
	public Player Player { get; private set; }

	public void SetPlayerOfSlot(Player player)
	{
		this.Player = player;
		if (Player != null)
		{ 
			this.gameObject.SetActive(true);
			flagImage.color = player.PlayerRealColor;
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}

	public void SetRankOfSlot(int rank)
	{
		rankText.text = "#" + (rank + 1).ToString();
		if (rank == 0)
		{
			rankText.text = "Winner!";
			backgroundImage.color = new Color(1, 0.99f, 0.2f);
		}
		else if (rank == 1)
		{
			backgroundImage.color = Color.white;
		}
		else
		{
			backgroundImage.color = new Color(0.5f, 0.3f, 0.15f);
		}
	}
}
