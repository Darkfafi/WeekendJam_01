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
				Destroy(killIcons[i].transform.parent.gameObject);
			}
		} 
		if(deathIcons.Count > 0)
		{
			for(int i = deathIcons.Count - 1; i >= 0; i--)
			{
				Destroy(deathIcons[i].transform.parent.gameObject);
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

		AnimateKillAndDeathIcons();
    }

	private Image CreateIcon(Player p, string iconTypeString)
	{
		GameObject iconObject = new GameObject(iconTypeString + "[Holder]");
		Image iconImage = new GameObject(iconTypeString).AddComponent<Image>();
		Transform parent = transform;
		switch(iconTypeString)
		{
			case ICON_TYPE_KILL:
				iconImage.sprite = killIconSprite;
				parent = killsIconHolderTransform;
                break;
			case ICON_TYPE_DEATH:
				iconImage.sprite = killerIconSprite;
				parent = deathsIconHolderTransform;
				break;
		}
		iconImage.color = p.PlayerRealColor;
		iconObject.AddComponent<RectTransform>().localScale = new Vector3(1,1,1);
		iconObject.gameObject.transform.SetParent(parent, false);
        iconImage.transform.SetParent(iconObject.transform, false);
		iconImage.rectTransform.sizeDelta = new Vector2(20, 20);
		return iconImage;
	}

	private void AnimateKillAndDeathIcons(float size = 20,float speed = 20f)
	{
		StartCoroutine(IconsAnimation(killIcons.ToArray(), size, speed));
		StartCoroutine(IconsAnimation(deathIcons.ToArray(), size, speed));
    }

	private IEnumerator IconsAnimation(Image[] icons, float size = 20, float speed = 20f)
	{
		yield return null;
		foreach (Image icon in icons)
		{
			icon.rectTransform.sizeDelta = new Vector2(0, 0);
		}

		bool reachedOverFlowAnim = false;
		bool animating = false;
		speed = (speed * 4);
		foreach (Image icon in icons)
		{
			animating = true;
			while (animating)
			{
				if (icon.rectTransform.sizeDelta.x < size * 1.25f && !reachedOverFlowAnim)
				{
					icon.rectTransform.sizeDelta = new Vector2(icon.rectTransform.sizeDelta.x + Time.deltaTime * speed, icon.rectTransform.sizeDelta.y + Time.deltaTime * speed);
				}
				else if (icon.rectTransform.sizeDelta.x >= size * 1.25f && !reachedOverFlowAnim)
				{
					reachedOverFlowAnim = true;
				}
				else if (reachedOverFlowAnim && icon.rectTransform.sizeDelta.x > size)
				{
					icon.rectTransform.sizeDelta = new Vector2(icon.rectTransform.sizeDelta.x - Time.deltaTime * speed, icon.rectTransform.sizeDelta.y - Time.deltaTime * speed);
				}
				else if (icon.rectTransform.sizeDelta.x < size)
				{
					icon.rectTransform.sizeDelta = new Vector2(size, size);
					reachedOverFlowAnim = false;
					animating = false;
				}
				yield return null;
			}
		}
	}
}
