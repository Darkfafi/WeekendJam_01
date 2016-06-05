using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Editor.ScreenGridEditor
{
	public class ScreenGridEditor : EditorWindow
	{
		private static EditorWindow window;
		private static GridDisplay gridDisplay = null;

		private SpriteRenderer tileObject;
		private Sprite tileSprite;
		private Transform parentObject = null;
		private bool createDrag = false;
		private bool deleteDrag = false;

		[MenuItem("Ramses/ScreenGridEditor/Activate")]
		public static void ShowWindow()
		{
			GUIContent newWindowContent = new GUIContent("ScreenGridEditor", "Tool to place tiles with art in a grid based environment");
			window = EditorWindow.GetWindow(typeof(ScreenGridEditor));
			window.titleContent = newWindowContent;
        }

		void OnDestroy()
		{
			if (gridDisplay != null)
			{
				DestroyGridDisplay();
            }
		}

		private void DestroyGridDisplay()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			GameObject.DestroyImmediate(gridDisplay.gameObject);
			gridDisplay = null;
		}

		private void OnGUI()
		{
			GUILayout.Label('\n' + "Left mouse button creates tiles snapped to the grid." + '\n' + "Right mouse button deletes tiles in selected parent" + '\n' + 
				"If no parent is given then this tool creates one instead." + '\n' + "WARNING: " + '\n' + "Do not run the game when tool is active! " + '\n' + "It will lose connection to the GridDisplayer" + '\n');

			tileObject = EditorGUILayout.ObjectField("Prefab Tile", tileObject, typeof(SpriteRenderer), false) as SpriteRenderer;
			tileSprite = EditorGUILayout.ObjectField("Tile Sprite", tileSprite, typeof(Sprite), false) as Sprite;
			parentObject = EditorGUILayout.ObjectField("Parent for Tile", parentObject, typeof(Transform), true) as Transform;

			if (gridDisplay != null)
			{
				gridDisplay.GridColor = EditorGUILayout.ColorField("Grid Color", gridDisplay.GridColor);
				gridDisplay.TilePrefab = tileObject;
				gridDisplay.TileArt = tileSprite;

                if (tileObject != null)
				{
					gridDisplay.SizeX = tileObject.bounds.size.x;
					gridDisplay.SizeY = tileObject.bounds.size.y;
				}
				else
				{
					gridDisplay.SizeX = 1;
					gridDisplay.SizeY = 1;
				}
			}

			if (GUILayout.Button((gridDisplay == null ? "Activate" : "Deactivate")))
			{
				SceneView.onSceneGUIDelegate -= OnSceneGUI;
				if (gridDisplay != null)
				{
					DestroyGridDisplay();
				}
				else
				{
					gridDisplay = new GameObject("{GridDisplayer}(Editor Object)").AddComponent<GridDisplay>();
					SceneView.onSceneGUIDelegate += OnSceneGUI;
				}
			}

		}

		private void OnSceneGUI(SceneView sceneView)
		{
			if (gridDisplay != null)
			{
				int cID = GUIUtility.GetControlID(FocusType.Passive);
				Event e = Event.current;
				Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
				Vector3 mousePos = ray.origin;
				Vector3 alignPos = new Vector3(Mathf.Floor((mousePos.x + gridDisplay.SizeX / 2) / gridDisplay.SizeX) * gridDisplay.SizeX,
													   Mathf.Floor((mousePos.y + gridDisplay.SizeY / 2) / gridDisplay.SizeY) * gridDisplay.SizeY, 0);
				if (e.isMouse && e.type == EventType.MouseDown)
				{
					GUIUtility.hotControl = cID;
					if (tileObject != null)
					{


						if (e.button == 0)
						{
							deleteDrag = false;
							createDrag = true;
						}
						else if (e.button == 1)
						{
							if (parentObject != null)
							{
								deleteDrag = true;
								createDrag = false;
							}
						}
					}
					else
					{
						Debug.LogWarning("Please link in a tile to create");
					}
				}

				if (createDrag)
				{
					if (parentObject == null)
					{
						parentObject = new GameObject(":-RootGridItems-:").transform;
					}
					DeleteTileOnPosition(alignPos);
					SpriteRenderer tile = Instantiate<SpriteRenderer>(tileObject);
					if (parentObject != null)
					{
						tile.transform.SetParent(parentObject);
					}
					tile.sprite = tileSprite;

					tile.transform.position = alignPos;
				}

				if(deleteDrag)
				{
					DeleteTileOnPosition(alignPos);
				}

				if (e.isMouse && e.type == EventType.MouseUp)
				{
					GUIUtility.hotControl = 0;
					createDrag = false;
					deleteDrag = false;
                }
			}
		}

		private void DeleteTileOnPosition(Vector3 pos)
		{
			if (parentObject != null)
			{
				int i = 0;
				while (i < parentObject.childCount)
				{
					Transform t = parentObject.GetChild(i);
					if (t.position == pos)
					{
						DestroyImmediate(t.gameObject);
						return;
					}
					i++;
				}
			}
		}
    }
}