using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;
using Ramses.Grid;
using System.Collections.Generic;
using Ramses.Entities;

public class ConAiGridManager : MonoBehaviour, IConfactory
{
	public Grid<AIGridNode> AIGrid { get; private set; }

	[SerializeField]
	private bool debugMode = false;

	private List<AIGridNode> waypointNodes = null;

	public void ConClear()
	{
		waypointNodes = null;
		AIGrid.ClearGrid();
    }

	public void ConStruct()
	{
		MassEntity gbi = ConfactoryFinder.Instance.Give<ConEntityDatabase>().GetAnyEntity<MassEntity>("GridBoundsItem");
		SpriteRenderer tileObject = Resources.Load<SpriteRenderer>("Tiles/BaseTile");
		AIGrid = new Grid<AIGridNode>(tileObject.bounds.size.x, 
			(gbi != null ? new Vector2(gbi.transform.position.x - gbi.Size.x * 0.5f, gbi.transform.position.y - gbi.Size.y * 0.5f) : new Vector2()));
		if(gbi != null)
		{
			AIGrid.SetGrid(Mathf.CeilToInt(SizeToTiles(gbi.Size.x, AIGrid.TileSize) * 1.1f),Mathf.CeilToInt(SizeToTiles(gbi.Size.y, AIGrid.TileSize) * 1.1f));
		}
		else
		{
			AIGrid.SetGrid(100, 100);
		}
    }

	public void OnSceneSwitch(int newSceneIndex)
	{
		ConfactoryFinder.Instance.Delete<ConAiGridManager>();
	}

	public void SetDebugWaypoints(AIGridNode[] nodes)
	{
		waypointNodes = new List<AIGridNode>(nodes);
	}

	private int SizeToTiles(float size, float tileSize)
	{
		int tiles = 0;
		while(size >= tileSize)
		{
			size -= tileSize;
			tiles++;
		}
		return tiles;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (debugMode)
		{
			if (AIGrid != null)
			{
				foreach (AIGridNode node in AIGrid.GetAllNodesArray())
				{
					Gizmos.color = new Color(1, 0, 0, 0.5f);
					bool checkPoint = false;
					if ((waypointNodes != null && waypointNodes.Count > 0 && waypointNodes.Contains(node)))
					{
						GizmosExtensions.DrawString(node.Data.J.ToString(), new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y + node.Size, 1), Color.white);
						Gizmos.color = Color.blue;
						checkPoint = true;
					}
					if (!checkPoint)
					{
						if (node.PositionX == 0 && node.PositionY == 0)
						{
							Gizmos.color = Color.green;
						}
						if (node.Data.IsGround())
						{
							Gizmos.color = Color.green;
						}
						else if (node.Data.IsSolid())
						{
							Gizmos.color = Color.red;
						}
						else
						{
							Gizmos.DrawWireCube(node.GetWorldPosition(), new Vector3(node.Size, node.Size, node.Size));
							continue;
						}
					}
					Gizmos.color = Gizmos.color.AlphaVersion(0.5f);
					Gizmos.DrawCube(node.GetWorldPosition(), new Vector3(node.Size, node.Size, node.Size));
				}
			}
		}
	}
#endif
}


public class AIGridNode : Node<AIGridNodeData>
{
	public AIGridNodeData Data { get; private set; }

	public override void SetNode(IGrid grid, int xPosition, int yPosition, float size)
	{
		base.SetNode(grid, xPosition, yPosition, size);
		Data = new AIGridNodeData(this);
		DataCheck();
	}

	public override AIGridNodeData DataCheck()
	{
		RaycastHit2D rHit = Physics2D.Raycast(GetWorldPosition(), Vector2.zero);
		Data.SetData(rHit);
		return Data;
	}
}

public class AIGridNodeData : INodeData
{
	private RaycastHit2D hit;
	private AIGridNode node;

	// costs
	public int G = 0;
	public int H = 0;
	public int J = 0; // Jump cost

	public float F { get { return G + H + J; } }

	public AIGridNode ParentNode = null;
	public AIGridNode[] Neighbours { get; private set; }

	public void ResetSearchData()
	{
		G = 0;
		H = 0;
		J = 0;
		ParentNode = null;
	}

	public AIGridNodeData(AIGridNode node)
	{
		this.node = node;
	}

	public void SetNeighbours(AIGridNode[] neighbours)
	{
		this.Neighbours = neighbours;
	}

	public void SetData(RaycastHit2D hit)
	{
		this.hit = hit;

	}

	public bool IsGround()
	{
		if (!IsSolid())
		{
			Grid<AIGridNode> gr = node.Grid as Grid<AIGridNode>;
			AIGridNode n = gr.GetNode(node.PositionX, node.PositionY - 1);
			if (n != null && n.Data.IsSolid())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSolid()
	{
		return hit.collider != null;
	}
}
