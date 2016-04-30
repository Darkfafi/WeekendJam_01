using UnityEngine;
using System.Collections;
using Ramses.Entities;

public class PickAbleObject : MonoEntity{

	public string ItemId {
		get { return itemId; }
	}

	[SerializeField] protected string itemId = "item";

	protected override void Awake()
	{
		base.Awake();
		AddTag(itemId);
	}

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
