﻿using UnityEngine;
using System.Collections;
using Ramses.Grid;
using System.Collections.Generic;

public class AIGridManager : MonoBehaviour
{
	public Grid<AIGridNode> AIGrid { get; private set; }
	[SerializeField] private SpriteRenderer tileUsed;
	[SerializeField] private bool debugMode = false;
	[SerializeField] private Character c;

	private List<AIGridNode> waypointNodes = null;

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
                if (Input.GetKey(KeyCode.Space))
				{
					waypointNodes = new List<AIGridNode>(AStarPlatforming.Search(AIGrid, (c != null ? AIGrid.GetNodeByWorldPointHit(c.transform.position).Position : new Vector2(10, 3)),
						AIGrid.GetNodeByWorldPointHit(Camera.main.ScreenToWorldPoint(Input.mousePosition)).Position, (c != null ? c.PlatformerMovement : null)));
				}

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