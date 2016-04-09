using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerSlot : MonoBehaviour
{
	public PlayerInfo PlayerOnSlot { get; private set; }
	public bool IsReady { get { return readyObject.activeSelf; } }
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
		PlayerOnSlot = player;

		if (PlayerOnSlot != null)
		{
			usedSlot.SetActive(true);
			unusedSlot.SetActive(false);
		}
		else
		{
			usedSlot.SetActive(false);
			SetReady(false);
			unusedSlot.SetActive(true);
		}
	}
	public void SetReady(bool readyState)
	{
		readyObject.SetActive(readyState);
	}
}
