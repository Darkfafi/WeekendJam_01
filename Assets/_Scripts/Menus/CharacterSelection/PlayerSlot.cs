using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerSlot : MonoBehaviour
{
	private PlayerInfo playerOnSlot = null;
 	[SerializeField]
	private GameObject usedSlot;
	[SerializeField]
	private GameObject unusedSlot;
	[SerializeField]
	private GameObject readyObject;
	[SerializeField]
	private Image colorFlag;


	public void SetPlayerForSlot(PlayerInfo player)
	{
		playerOnSlot = player;

		if (playerOnSlot != null)
		{
			usedSlot.SetActive(true);
			unusedSlot.SetActive(false);
		}
		else
		{
			usedSlot.SetActive(false);
			unusedSlot.SetActive(true);
		}
	}

	void Update()
	{
		
	}
}
