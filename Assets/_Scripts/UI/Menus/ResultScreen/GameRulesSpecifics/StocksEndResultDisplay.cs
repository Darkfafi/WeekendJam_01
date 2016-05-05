using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ramses.Confactory;

public class StocksEndResultDisplay : MonoBehaviour {

	private const string ICON_TYPE_DEATH = "DeathTypeIcon";
	private const string ICON_TYPE_KILL = "KillTypeIcon";

	[SerializeField] private Sprite killIconSprite;
	[SerializeField] private Sprite killerIconSprite;

	[SerializeField] private Text killsText;
	[SerializeField] private Transform killsIconHolderTransform;

	[SerializeField] private Text deathsText;
	[SerializeField] private Transform deathsIconHolderTransform;

	private List<Image> killIcons = new List<Image>();
	private List<Image> deathIcons = new List<Image>();

	private ConGameBattleHistoryLog conGameBattleHistory;

	private void Awake()
	{
		conGameBattleHistory = ConfactoryFinder.Instance.Give<ConGameBattleHistoryLog>();
	}

	//TODO Animate all the kills and deaths in the screen with a coroutine.
	public void DisplayPlayerResult(Player player)
	{
		if (killIcons.Count > 0)
		{
			for (int i = killIcons.Count - 1; i >= 0; i--)
			{
				Destroy(killIcons[i].gameObject);
			}
		} 
		if(deathIcons.Count > 0)
		{
			for(int i = deathIcons.Count - 1; i >= 0; i--)
			{
				Destroy(deathIcons[i].gameObject);
			}
		}

		Player[] allKillers = conGameBattleHistory.GetAllKillersOfPlayer(player);
		Player[] allKilled = conGameBattleHistory.GetAllKillsOfPlayer(player);

		foreach(Player p in allKilled)
		{
			killIcons.Add(CreateIcon(p, ICON_TYPE_KILL));
		}

		killsText.text = "Kills: " + allKilled.Length;

		foreach(Player p in allKillers)
		{
			deathIcons.Add(CreateIcon(p, ICON_TYPE_DEATH));
		}

		deathsText.text = "Deaths: " + allKillers.Length;
	}


	private Image CreateIcon(Player p, string iconTypeString)
	{
		Image iconImage = new GameObject(iconTypeString).AddComponent<Image>();
		Transform parent = transform;
		switch(iconTypeString)
		{
			case ICON_TYPE_KILL:
				iconImage.sprite = killerIconSprite;
				parent = killsIconHolderTransform;
                break;
			case ICON_TYPE_DEATH:
				iconImage.sprite = killerIconSprite;
				parent = deathsIconHolderTransform;
				break;
		}
		iconImage.color = p.PlayerRealColor;
		iconImage.transform.SetParent(parent, false);
		return iconImage;
	}

}
