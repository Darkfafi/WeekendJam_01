using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPlayerInfos : MonoBehaviour {

	[SerializeField]
	private UIPlayerInfo[] allUIPlayerInfos;

	protected void Awake()
	{
		ConActivePlayers activePlayers = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConActivePlayers>();
		Player[] allPlayers = activePlayers.GetAllPlayers();

		for (int i = 0; i < allPlayers.Length; i++)
		{
			allUIPlayerInfos[i].SetLinkedPlayer(allPlayers[i]);
        }
	}

	public UIPlayerInfo[] GetUIPlayerInfos(bool playerLinkedOnly = false)
	{
		List<UIPlayerInfo> infosToReturn = new List<UIPlayerInfo>();

		if (playerLinkedOnly == true)
		{
			foreach (UIPlayerInfo uiPInfo in allUIPlayerInfos)
			{
				if (uiPInfo.LinkedPlayer != null)
				{
					infosToReturn.Add(uiPInfo);
				}
			}
		}
		else
		{
			infosToReturn = new List<UIPlayerInfo>(allUIPlayerInfos);
        }

		return infosToReturn.ToArray();
	}

	public UIPlayerInfo GetUIPlayerInfo(Player player)
	{
		UIPlayerInfo infoToReturn = null;
		foreach(UIPlayerInfo uiPInfo in allUIPlayerInfos)
		{
			if(uiPInfo.LinkedPlayer == player)
			{
				infoToReturn = uiPInfo;
				break;
			}
		}
		return infoToReturn;
	}

	public UIPlayerInfo GetUIPlayerInfo(int index)
	{
		return allUIPlayerInfos[index];
    }
}
