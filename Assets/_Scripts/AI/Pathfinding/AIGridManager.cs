using UnityEngine;
using System.Collections;
using Ramses.Grid;

public class AIGridManager : MonoBehaviour
{
	public Grid<AIGridNode> AIGrid { get; private set; }
	[SerializeField] private SpriteRenderer tileUsed;

	private void Awake()
	{
		AIGrid = new Grid<AIGridNode>(tileUsed.bounds.size.x);
		AIGrid.SetGrid(100, 100);
	}


	private void OnDrawGizmos()
	{
		if (AIGrid != null)
		{
			foreach(AIGridNode node in AIGrid.GetAllNodesArray())
			{
				Gizmos.color = Color.red;
				if(node.PositionX == 0 && node.PositionY == 0)
				{
					Gizmos.color = Color.green;
				}
				Gizmos.DrawWireCube(node.GetWorldPosition(), new Vector3(node.Size, node.Size, node.Size));
			}
		}
	}
}

public class AIGridNode : Node<AIGridNodeData>
{

}

public class AIGridNodeData : INodeData
{

}