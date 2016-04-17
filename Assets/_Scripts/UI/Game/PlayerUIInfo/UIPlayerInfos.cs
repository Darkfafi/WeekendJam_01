using UnityEngine;
using System.Collections;

public class UIPlayerInfos : MonoBehaviour {

	[SerializeField]
	private UIPlayerInfo[] allUIPlayerInfos;

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
