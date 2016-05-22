using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIPlayerInfo : MonoBehaviour {

	public enum ScoreIndications
	{
		None,
		Stocks,
		Score
	}

	public Player LinkedPlayer { get; private set; }
	public TextIndicationUI CurrentIndication { get; private set; }
	public ScoreIndications CurrentIndicationType { get; private set; }

	public Image PortraitImage { get { return portraitImage; } }
	public Image BackgroundImage { get { return backgroundImage; } }

	[SerializeField]
	private Image portraitImage;

	[SerializeField]
	private Image backgroundImage;

	//Types
	[SerializeField]
	private TextIndicationUI stocksIndication;

	[SerializeField]
	private TextIndicationUI scoreIndication;

	public void SetLinkedPlayer(Player player)
	{
		LinkedPlayer = player;
		if (LinkedPlayer != null)
		{
			backgroundImage.color = ColorHandler.ColorsToColor(player.PlayerColor);
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}
    }

	public void SetScoreIndication(ScoreIndications indication)
	{
		stocksIndication.gameObject.SetActive(false);
		scoreIndication.gameObject.SetActive(false);
		CurrentIndication = null;
        switch (indication)
		{
			case ScoreIndications.Score:
				scoreIndication.gameObject.SetActive(true);
				CurrentIndication = scoreIndication;
                break;
			case ScoreIndications.Stocks:
				stocksIndication.gameObject.SetActive(true);
				CurrentIndication = stocksIndication;
                break;
		}
		CurrentIndicationType = indication;
	}
}
