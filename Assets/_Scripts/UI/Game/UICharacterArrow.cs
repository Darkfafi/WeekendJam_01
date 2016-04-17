using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICharacterArrow : MonoBehaviour {

	public Player LinkedPlayer { get { return linkedPlayer; }
		set
		{
			linkedPlayer = value;
			if(linkedPlayer != null)
			{
				arrowImage.color = ColorHandler.ColorsToColor(linkedPlayer.PlayerColor);
            }
		}
	}
	private Player linkedPlayer = null;
	private Vector3 pos = new Vector3();
	[SerializeField] private Image arrowImage;

	void Update()
	{
		if (LinkedPlayer != null)
		{
			if (LinkedPlayer.PlayerCharacter != null && LinkedPlayer.PlayerCharacter.CharacterCollider != null)
			{
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
				pos = LinkedPlayer.PlayerCharacter.transform.position;
				pos.y += linkedPlayer.PlayerCharacter.CharacterCollider.bounds.size.y;

				transform.position =  pos;
			}
			else
			{
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}
}
