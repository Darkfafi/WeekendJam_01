using UnityEngine;
using System.Collections;
using Ramses.Grid;

public class AIGridManager : MonoBehaviour
{
	public Grid<AIGridNode> AIGrid { get; private set; }
	[SerializeField] private SpriteRenderer tileUsed;
	[SerializeField] private bool debugMode = false;

	private void Awake()
	{
		AIGrid = new Grid<AIGridNode>(tileUsed.bounds.size.x);
		AIGrid.SetGrid(100, 100);
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
					if (node.PositionX == 0 && node.PositionY == 0)
					{
						Gizmos.color = Color.green;
					}
					if (node.Data.Solid)
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

	public AIGridNodeData(AIGridNode node)
	{
		this.node = node;
    }

	public void SetData(RaycastHit2D hit)
	{
		this.hit = hit;
	}
}