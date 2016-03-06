using UnityEngine;
using System.Collections;

public class ObjectPicker : MonoBehaviour {
	
	public PickUpInfo PickUpObject<T>(Vector2 direction, Vector2 startRayPosition, float pickUpLength, bool destroyOnPickUp = true) where T : PickAbleObject
	{
		PickUpInfo info;
		RaycastHit2D hit = Physics2D.Raycast(startRayPosition, direction, pickUpLength, Layers.LayerMaskSeeOnly(Layers.OBJECTS));
		Debug.DrawRay(startRayPosition, direction * pickUpLength, Color.green);
		string idObject = "";
		PickAbleObject pickedUpGameObject = null;
		
        if (hit.collider != null)
		{
			if (hit.collider.gameObject.GetComponent<T>() != null)
			{
				hit.collider.gameObject.GetComponent<T>().PickUpObject(out pickedUpGameObject, out idObject, destroyOnPickUp);
			}
		}
		info = new PickUpInfo(pickedUpGameObject, idObject);
		return info;
	}
}
public class PickUpInfo
{
	public readonly PickAbleObject objectPickedUp;
	public readonly string objectId;

	public PickUpInfo(PickAbleObject objectPickedUp, string id)
	{
		this.objectPickedUp = objectPickedUp;
		this.objectId = id;
	}
}