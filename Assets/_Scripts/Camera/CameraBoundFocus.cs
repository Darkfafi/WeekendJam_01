using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Entities;
using Ramses.Confactory;

public class CameraBoundFocus : MonoBehaviour
{

	// Options 
	[SerializeField]
	private float MinZoom = 2.8f;
	[SerializeField]
	private float lerpPositionSpeed = 4f;
	[SerializeField]
	private float lerpZoomSpeed = 4f;
	[SerializeField]
	private Vector2 offset;

	private float MaxZoom;
	private float Left;
	private float Right;
	private float Top;
	private float Bottom;


	private Camera thisCamera;
	private CameraMassEntity boundsEntity;
	private List<string> tagsToFocusOn = new List<string>();
	private ConEntityDatabase conEntityDatabase;

	void Awake () {
		thisCamera = gameObject.GetComponent<Camera>();
		conEntityDatabase = ConfactoryFinder.Instance.Give<ConEntityDatabase>();
	}

	void Start()
	{
		boundsEntity = conEntityDatabase.GetAnyEntity<CameraMassEntity>("CamBoundsItem");
		Refresh();
	}

	void FixedUpdate ()
	{
		CameraMassEntity[] entities = conEntityDatabase.GetEntities<CameraMassEntity>("CamFocusItem");

		if (entities.Length > 0)
		{
			Vector2 centerOfObjects = entities[0].Position;
			Vector2 diff;
			float xDist = entities[0].Size.x;
			float yDist = entities[0].Size.y;

			for (int i = entities.Length - 1; i >= 1; i--)
			{
				diff = entities[i].Position - centerOfObjects;
                centerOfObjects += (diff * 0.5f);
            }

			centerOfObjects += offset;

			Debug.DrawLine(entities[0].Position, centerOfObjects);

			for (int i = entities.Length - 1; i >= 0; i--)
			{
				diff = entities[i].Position - centerOfObjects;
                diff.x = Mathf.Abs(diff.x);
				diff.y = Mathf.Abs(diff.y);
				xDist = (diff.x > xDist) ? diff.x : xDist;
				yDist = (diff.y > yDist) ? diff.y : yDist;
			}

			Zoom(Mathf.Lerp(thisCamera.orthographicSize, xDist + yDist, Time.deltaTime * lerpZoomSpeed));
			RefreshBounds();

			
			centerOfObjects.x = Mathf.Clamp(centerOfObjects.x, Left, Right);
			centerOfObjects.y = Mathf.Clamp(centerOfObjects.y, Bottom, Top);

			Debug.DrawLine(entities[0].Position, centerOfObjects, Color.red);

			Vector2 lerpedCamPos = Vector2.Lerp(thisCamera.transform.position, centerOfObjects, Time.deltaTime * lerpPositionSpeed);
            thisCamera.transform.position = new Vector3(Mathf.Clamp(lerpedCamPos.x, Left, Right), Mathf.Clamp(lerpedCamPos.y, Bottom, Top), - 10);
		}
	}

	public void Zoom(float value)
	{
		thisCamera.orthographicSize = value;
		thisCamera.orthographicSize = Mathf.Clamp(thisCamera.orthographicSize, MinZoom, MaxZoom);
	}

	private void Refresh()
	{
		//calculate current screen ratio
		float w = Screen.width / boundsEntity.Size.x;
		float h = Screen.height / boundsEntity.Size.y;
		float ratio = w / h;
		float ratio2 = h / w;
		if (ratio2 > ratio)
		{
			MaxZoom = (boundsEntity.Size.y / 2);
		}
		else
		{
			MaxZoom = (boundsEntity.Size.y / 2);
			MaxZoom /= ratio;
		}

		thisCamera.orthographicSize = MaxZoom;

		RefreshBounds();
	}

	private void RefreshBounds()
	{
		var vertExtent = thisCamera.orthographicSize;
		var horzExtent = vertExtent * Screen.width / Screen.height;

		Left = horzExtent - boundsEntity.Size.x / 2.0f + boundsEntity.Position.x;
		Right = boundsEntity.Size.x / 2.0f - horzExtent + boundsEntity.Position.x;
		Bottom = vertExtent - boundsEntity.Size.y / 2.0f + boundsEntity.Position.y;
		Top = boundsEntity.Size.y / 2.0f - vertExtent + boundsEntity.Position.y;
	}
}
