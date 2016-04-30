using UnityEngine;
using System.Collections;
using Ramses.Entities;

public class MassEntity : MonoEntity {

	public Vector2 Position { get { return offset.Vec3Add(transform.position); } }
	public Vector2 Size { get { return size; } }

	[SerializeField] private Vector2 offset;
	[SerializeField] private Vector2 size = new Vector2(1, 1);
	[SerializeField] private Color color = Color.green;
	[SerializeField] private bool solidDisplay = false;

	public Vector2 PositionForCam(Camera cam)
	{
		return cam.WorldToScreenPoint(Position);
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		if (solidDisplay)
		{
			Gizmos.DrawCube(Position, Size);
		}
		else
		{
			Gizmos.DrawWireCube(Position, Size);
		}
	}
}
