using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowPlayerIndicator : MonoBehaviour {

	[SerializeField] GameHandler gameHandler;
	[SerializeField] GameObject playerArrowItem;

	private List<UICharacterArrow> arrowsActive = new List<UICharacterArrow>();

	void OnEnable()
	{
		gameHandler.PlayerCharacterSpawnedEvent -= OnPlayerCharacterSpawnedEvent;
		gameHandler.PlayerCharacterSpawnedEvent += OnPlayerCharacterSpawnedEvent;
    }

	void OnDisable()
	{
		gameHandler.PlayerCharacterSpawnedEvent -= OnPlayerCharacterSpawnedEvent;
	}

	private void OnPlayerCharacterSpawnedEvent(Player player)
	{
		CreateArrowForPlayer(player);
	}

	private void CreateArrowForPlayer(Player player)
	{
		UICharacterArrow arrow = GetActiveArrowForPlayer(player);
		if(arrow != null)
		{
			return;
		}

		arrow = Instantiate<GameObject>(playerArrowItem).GetComponent<UICharacterArrow>();
		arrow.transform.SetParent(this.transform,false);
		arrow.LinkedPlayer = player;
    }

	private UICharacterArrow GetActiveArrowForPlayer(Player player)
	{
		foreach(UICharacterArrow arrow in arrowsActive)
		{
			if(arrow.LinkedPlayer == player)
			{
				return arrow;
			}
		}
		return null;
	}
}
