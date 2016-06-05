using UnityEngine;
using System.Collections;

namespace Editor.TilesGenerator
{
	public class TilesGenerator : ScriptableObject
	{

		public void GenerateTileSet(int width, int height, string setName, SpriteRenderer tilePrefab, TileSetSpriteInfo spritesInfo, int layer = -1)
		{
			SpriteRenderer tileObject = null;
			if (tilePrefab != null)
			{
				GameObject parent = new GameObject(setName + "(size[" + width + "x" + height + "])");
				for (int yRow = 0; yRow < height; yRow++)
				{
					for (int xRow = 0; xRow < width; xRow++)
					{
						tileObject = GameObject.Instantiate<SpriteRenderer>(tilePrefab);
						tileObject.sprite = GetCorrectSprite(xRow, yRow, width, height, spritesInfo);
						tileObject.transform.position = new Vector3(xRow * (tileObject.sprite.bounds.size.x * tilePrefab.transform.localScale.x), yRow * (tileObject.sprite.bounds.size.y * tilePrefab.transform.localScale.y));
						tileObject.transform.SetParent(parent.transform);
					}
				}
			}
			else
			{
				Debug.LogError("Please give a Tile Prefab to generate the tiles with");
			}
		}

		private Sprite GetCorrectSprite(int x, int y, int width, int height, TileSetSpriteInfo info)
		{
			Sprite s = info.CenterTileSprite;

			width -= 1;
			height -= 1;

			if (x != 0 && x != width)
			{
				if (y == height)
				{
					s = info.TopTileSprite;
				}
				else if (y == 0)
				{
					s = info.BottomTileSprite;
				}
				else
				{
					s = info.CenterTileSprite;
				}
			}
			else if (x == 0)
			{
				if (y == height)
				{
					s = info.LeftTopTileSprite;
				}
				else if (y == 0)
				{
					s = info.BottomLeftTileSprite;
				}
				else
				{
					s = info.LeftTileSprite;
				}
			}
			else
			{
				if (y == height)
				{
					s = info.RightTopTileSprite;
				}
				else if (y == 0)
				{
					s = info.BottomRightTileSprite;
				}
				else
				{
					s = info.RightTileSprite;
				}
			}
			return s;
		}
	}

	public class TileSetSpriteInfo
	{
		public Sprite TopTileSprite = null;
		public Sprite LeftTopTileSprite = null;
		public Sprite RightTopTileSprite = null;

		public Sprite CenterTileSprite = null;
		public Sprite LeftTileSprite = null;
		public Sprite RightTileSprite = null;

		public Sprite BottomTileSprite = null;
		public Sprite BottomLeftTileSprite = null;
		public Sprite BottomRightTileSprite = null;

		public void SetTileSprites(Sprite center)
		{
			SetTileSprites(center, center, center);
		}

		public void SetTileSprites(Sprite top, Sprite down, Sprite center)
		{
			SetTileSprites(center, top, center, down, center);
		}

		public void SetTileSprites(Sprite left, Sprite top, Sprite right, Sprite down, Sprite center)
		{
			SetTileSprites(left, top, right, down, center, top, top, down, down);
		}

		public void SetTileSprites(Sprite left, Sprite top, Sprite right, Sprite down, Sprite center,
			Sprite leftTop, Sprite rightTop, Sprite leftDown, Sprite rightDown)
		{
			TopTileSprite = top;
			LeftTopTileSprite = leftTop;
			RightTopTileSprite = rightTop;

			CenterTileSprite = center;
			LeftTileSprite = left;
			RightTileSprite = right;

			BottomTileSprite = down;
			BottomLeftTileSprite = leftDown;
			BottomRightTileSprite = rightDown;
		}
	}
}