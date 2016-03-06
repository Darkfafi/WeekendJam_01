using UnityEngine;
using System.Collections;

public class PickAbleObject : MonoBehaviour {

	public string ItemId {
		get { return itemId; }
	}

	[SerializeField] protected string itemId = "item";

	public void PickUpObject(out PickAbleObject objectPickedUp, out string id, bool destroyOnPickUp = true)
	{
		id = itemId;
        objectPickedUp = this;
		if (destroyOnPickUp)
		{
			Destroy(this.gameObject);
		}
    }
}
