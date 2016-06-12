using UnityEngine;
using System.Collections;

namespace Editor.ScreenGridEditor
{
	[ExecuteInEditMode]
	public class GridDisplay : MonoBehaviour
	{
		public float SizeY = 1;
		public float SizeX = 1;
		public Color GridColor = Color.yellow;
		public SpriteRenderer TilePrefab;
		public Sprite TileArt; 

		private void Update()
		{
			if(Input.GetMouseButtonDown(0))
			{
				SpawnTile();
			}
		}

		private void SpawnTile()
		{
			SpriteRenderer r =  Instantiate<SpriteRenderer>(TilePrefab);
			r.sprite = TileArt;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = GridColor;
			Vector3 pos = Camera.current.transform.position;

			for (float y = pos.y - 100; y < pos.y + 100; y += SizeY)
			{
				Gizmos.DrawLine(new Vector3(-10000f, Mathf.Floor(y / SizeY) * SizeY + (SizeY / 2), 0), new Vector3(10000f, Mathf.Floor(y / SizeY) * SizeY + (SizeY / 2), 0));
			}

			for (float x = pos.x - 100; x < pos.x + 100; x += SizeX)
			{
				Gizmos.DrawLine(new Vector3(Mathf.Floor(x / SizeX) * SizeX + (SizeX / 2), -10000f, 0), new Vector3(Mathf.Floor(x / SizeX) * SizeX + (SizeX / 2), 10000f, 0));
			}
		}
	}
}