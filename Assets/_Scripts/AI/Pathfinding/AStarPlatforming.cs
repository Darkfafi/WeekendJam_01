using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Grid;

public class AStarPlatforming {


	// Keep in mind the platformer2D movement and the height of the character. Also gravity

	public static AIGridNode[] Search(Grid<AIGridNode> aiGrid, Vector2 startGridPos, Vector2 endGridPos, PlatformerMovement2D platformerMovement)
	{
		List<AIGridNode> allWaypointNodes = new List<AIGridNode>();
		AIGridNode currentNode = aiGrid.GetNode(startGridPos);
		AIGridNode endNode = aiGrid.GetNode(endGridPos);

		if (currentNode != null && !currentNode.Data.Solid 
			&& endNode != null && !endNode.Data.Solid)
		{
			ResetGrid(aiGrid);

			List<AIGridNode> openNodes = new List<AIGridNode>();
			List<AIGridNode> closedNodes = new List<AIGridNode>();

			AIGridNode neighbourChecking = null;

			openNodes.Add(currentNode);
			while (openNodes.Count > 0)
			{
				openNodes.Sort(SortOnF);
				currentNode = openNodes[0];
				if (currentNode.Position.Equals(endGridPos))
				{
					while (currentNode.Data.ParentNode != null)
					{
						allWaypointNodes.Add(currentNode);
						currentNode = currentNode.Data.ParentNode;
					}
					allWaypointNodes.Reverse();

					return allWaypointNodes.ToArray(); // Return path with all waypoints
				}

				openNodes.Remove(currentNode);
				closedNodes.Add(currentNode);

				if (currentNode.Data.Neighbours == null)
				{
					currentNode.Data.SetNeighbours(GetNeighbourNodes(aiGrid, currentNode));
				}

				for (int i = 0; i < currentNode.Data.Neighbours.Length; i++)
				{
					neighbourChecking = currentNode.Data.Neighbours[i];

					if (closedNodes.Contains(neighbourChecking) || neighbourChecking.Data.Solid)
					{
						continue;
					}

					neighbourChecking.Data.G = DistanceValueCheck(startGridPos, neighbourChecking.Position);

					if (!openNodes.Contains(neighbourChecking))
					{
						neighbourChecking.Data.H = DistanceValueCheck(neighbourChecking.Position, endGridPos);
						openNodes.Add(neighbourChecking);
						neighbourChecking.Data.ParentNode = currentNode;
					}
				}
			}
		}
		return allWaypointNodes.ToArray();
	}

	private static int DistanceValueCheck(Vector2 start, Vector2 end)
	{
		int xValue = (int)Mathf.Abs(end.x - start.x);
		int yValue = (int)Mathf.Abs(end.y - start.y);

		return (xValue + yValue);
	}

	private static int SortOnF(AIGridNode a, AIGridNode b)
	{
		if (a.Data.F > b.Data.F || a.Data.F == b.Data.F && a.Data.H > b.Data.H)
		{
			return 1;
		}
		else
		{
			return -1;
		}
	}

	private static void ResetGrid(Grid<AIGridNode> aiGrid)
	{
		AIGridNode[] allNodes = aiGrid.GetAllNodesArray();
        for (int i = 0; i < allNodes.Length; i++)
		{
			allNodes[i].Data.ResetSearchData();
		}
	}

	private static AIGridNode[] GetNeighbourNodes(Grid<AIGridNode> aiGrid, AIGridNode node)
	{
		List<AIGridNode> nodes = new List<AIGridNode>();
		int x = node.PositionX;
		int y = node.PositionY;

		if (aiGrid.GetNode(x - 1, y) != null)
		{
			nodes.Add(aiGrid.GetNode(x - 1, y));
		}
		if (aiGrid.GetNode(x + 1, y) != null)
		{
			nodes.Add(aiGrid.GetNode(x + 1, y));
		}
		if (aiGrid.GetNode(x, y - 1) != null)
		{
			nodes.Add(aiGrid.GetNode(x, y - 1));
		}
		if (aiGrid.GetNode(x, y + 1) != null)
		{
			nodes.Add(aiGrid.GetNode(x, y + 1));
		}

		return nodes.ToArray();
	}

	private static bool CheckPassableHeightNode(Grid<AIGridNode> aiGrid, AIGridNode node, float height)
	{
		AIGridNode checkingNode = node;
        while (height > 0)
		{
			if (checkingNode != null && !checkingNode.Data.Solid)
			{
				height -= checkingNode.Size;
				checkingNode = aiGrid.GetNode(checkingNode.PositionX, checkingNode.PositionY + 1);
			}
			else
			{
				return false;
			}
		}

		return true;
	}
}
