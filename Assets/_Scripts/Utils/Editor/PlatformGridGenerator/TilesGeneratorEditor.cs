using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Editor.TilesGenerator
{
	public class TilesGeneratorEditor : EditorWindow
	{

		private static EditorWindow window;
		private static TilesGenerator generator;

		// Data Editor
		private int height;
		private int width;
		private int layer = -1;
		private string chunkName = "TileSet";

		private SpriteRenderer tilePrefab = null;

		private Sprite topTileSprite = null;
		private Sprite leftTopTileSprite = null;
		private Sprite rightTopTileSprite = null;

		private Sprite centerTileSprite = null;
		private Sprite leftTileSprite = null;
		private Sprite rightTileSprite = null;

		private Sprite bottomTileSprite = null;
		private Sprite bottomLeftTileSprite = null;
		private Sprite bottomRightTileSprite = null;

		private TileSetSpriteInfo infoSprites = new TileSetSpriteInfo();

		private Vector2 scrollpos = Vector2.zero;

		[MenuItem("Ramses/TilesGenerator/Show Window %i")]
		public static void ShowWindow()
		{
			GUIContent newWindowContent = new GUIContent("TileGenerator", "Tool to generate a series of tiles in the editor");
			window = EditorWindow.GetWindow(typeof(TilesGeneratorEditor));
			window.titleContent = newWindowContent;
		}

		private void OnGUI()
		{
			GUILayout.Label("This will generate an object with as childeren tiles correctly placed in width and height" + '\n' + "* == Mandatory");
			scrollpos = GUILayout.BeginScrollView(scrollpos, false, true);

			width = EditorGUILayout.IntField("*width: ", width);
			height = EditorGUILayout.IntField("*height: ", height);
			chunkName = EditorGUILayout.TextField("Name of Set: ", chunkName);
			layer = EditorGUILayout.IntField("Layer: ", layer);
			tilePrefab = EditorGUILayout.ObjectField("*Prefab Tile", tilePrefab, typeof(SpriteRenderer), false) as SpriteRenderer;
			centerTileSprite = EditorGUILayout.ObjectField("*Center Tile", centerTileSprite, typeof(Sprite), false) as Sprite;
			topTileSprite = EditorGUILayout.ObjectField("(1)*Top Tile", topTileSprite, typeof(Sprite), false) as Sprite;
			bottomTileSprite = EditorGUILayout.ObjectField("(1)*Bottom Tile", bottomTileSprite, typeof(Sprite), false) as Sprite;

			leftTileSprite = EditorGUILayout.ObjectField("(2)*Left Tile", leftTileSprite, typeof(Sprite), false) as Sprite;
			rightTileSprite = EditorGUILayout.ObjectField("(2)*Right Tile", rightTileSprite, typeof(Sprite), false) as Sprite;

			leftTopTileSprite = EditorGUILayout.ObjectField("(3)*Top Left Tile", leftTopTileSprite, typeof(Sprite), false) as Sprite;
			rightTopTileSprite = EditorGUILayout.ObjectField("(3)*Top Right Tile", rightTopTileSprite, typeof(Sprite), false) as Sprite;
			bottomLeftTileSprite = EditorGUILayout.ObjectField("(3)*Bottom Left Tile", bottomLeftTileSprite, typeof(Sprite), false) as Sprite;
			bottomRightTileSprite = EditorGUILayout.ObjectField("(3)*Bottom Right Tile", bottomRightTileSprite, typeof(Sprite), false) as Sprite;

			if (leftTopTileSprite != null && rightTopTileSprite != null && bottomLeftTileSprite != null && bottomRightTileSprite != null)
			{
				infoSprites.SetTileSprites(leftTileSprite, topTileSprite, rightTileSprite
					, bottomTileSprite, centerTileSprite, leftTopTileSprite, rightTopTileSprite, bottomLeftTileSprite, bottomRightTileSprite);
			}
			else if (leftTileSprite != null && rightTileSprite != null)
			{
				infoSprites.SetTileSprites(leftTileSprite, topTileSprite, rightTileSprite, bottomTileSprite, centerTileSprite);
			}
			else
			{
				infoSprites.SetTileSprites(topTileSprite, bottomTileSprite, centerTileSprite);
			}

			if (GUILayout.Button("Generate"))
			{
				if (generator == null)
				{
					generator = new TilesGenerator();
				}
				generator.GenerateTileSet(width, height, chunkName, tilePrefab, infoSprites, layer);
			}
			GUILayout.EndScrollView();
		}
	}
}