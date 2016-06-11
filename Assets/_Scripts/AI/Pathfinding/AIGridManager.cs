using UnityEngine;
using System.Collections;
using Ramses.Grid;
using System.Collections.Generic;

public class AIGridManager : MonoBehaviour
{
	public Grid<AIGridNode> AIGrid { get; private set; }
	[SerializeField] private SpriteRenderer tileUsed;
	[SerializeField] private bool debugMode = false;
	[SerializeField] private Character c;
	private void Awake()
	{
		AIGrid = new Grid<AIGridNode>(tileUsed.bounds.size.x);
		AIGrid.SetGrid(35, 15);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (debugMode)
		{
			if (AIGrid != null)
			{
				List<AIGridNode> waypointNodes = null;
                if (Input.GetKey(KeyCode.Space))
				{
					waypointNodes = new List<AIGridNode>(AStarPlatforming.Search(AIGrid, (c != null ? AIGrid.GetNodeByWorldPointHit(c.transform.position).Position : new Vector2(10, 3)),
						AIGrid.GetNodeByWorldPointHit(Camera.main.ScreenToWorldPoint(Input.mousePosition)).Position, (c != null ? c.PlatformerMovement : null)));
				}

				foreach (AIGridNode node in AIGrid.GetAllNodesArray())
				{
					Gizmos.color = new Color(1, 0, 0, 0.5f);
					if (node.PositionX == 0 && node.PositionY == 0)
					{
						Gizmos.color = Color.green;
					}
					if (node.Data.Solid || (waypointNodes != null && waypointNodes.Count > 0 && waypointNodes.Contains(node))) 
					{
						Gizmos.DrawCube(node.GetWorldPosition(), new Vector3(node.Size, node.Size, node.Size));
					}
					else
					{
						Gizmos.DrawWireCube(node.GetWorldPosition(), new Vector3(node.Size, node.Size, node.Size));
					}
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
	public bool Solid { get { return (hit.collider != null); } }

	private RaycastHit2D hit;
	private AIGridNode node;

	// costs
	public float G = 0;
	public float H = 0;
	public float F { get { return G + H; } }
	public AIGridNode ParentNode = null;
	public AIGridNode[] Neighbours { get; private set; }

	public void ResetSearchData()
	{
		G = 0;
		H = 0;
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
}