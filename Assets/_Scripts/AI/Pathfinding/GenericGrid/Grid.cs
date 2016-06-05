using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Ramses.Grid
{
	public class Grid<T> : IGrid where T : class, INode
	{
		public float TileSize { get; private set; }

		private List<List<T>> allNodes_YX_Oder = new List<List<T>>();

		public Grid(float tileSize)
		{
			this.TileSize = tileSize;
		}

		public void SetGrid(int width, int height)
		{
			T node = null;
			List<T> yRowList;
			for (int yRow = 0; yRow < height; yRow++)
			{
				yRowList = new List<T>();
				allNodes_YX_Oder.Add(yRowList);
				for (int xRow = 0; xRow < width; xRow++)
				{
					node = Activator.CreateInstance<T>();
					node.SetNode(this, xRow, yRow, TileSize);
					yRowList.Add(node);
				}
			}
		}

		public T GetNode(int x, int y)
		{
			T nodeToReturn = null;

			if (x >= 0 && y >= 0 && x < allNodes_YX_Oder.Count && y < allNodes_YX_Oder[x].Count)
			{
				if (allNodes_YX_Oder[x] != null && allNodes_YX_Oder[x][y] != null)
				{
					nodeToReturn = allNodes_YX_Oder[x][y];
				}
			}

			return nodeToReturn;
		}

		public T GetNodeByWorldPointHit(Vector2 hitPosition)
		{
			T[] ar = GetAllNodesArray();
			for(int i = 0; i < ar.Length; i++)
			{
				if(ar[i].WorldPositionInBounds(hitPosition))
				{
					return ar[i];
				}
			}
			return null;
		}

		public Vector2 GetNodeWorldPosition(int x, int y)
		{
			return GetNode(x, y).GetWorldPosition();
		}

		public T GetNode(Vector2 gridPosition)
		{
			return GetNode((int)gridPosition.x, (int)gridPosition.y);
		}

		public T[] GetAllNodesArray()
		{
			List<T> aN = new List<T>();
			for(int i = 0; i < allNodes_YX_Oder.Count; i++)
			{
				aN.AddRange(allNodes_YX_Oder[i]);
			}
			return aN.ToArray();
		}

		INode IGrid.GetNode(int x, int y)
		{
			return GetNode(x, y);
		}

		INode IGrid.GetNodeByWorldPointHit(Vector2 hitPosition)
		{
			throw new NotImplementedException();
		}

		INode IGrid.GetNode(Vector2 gridPosition)
		{
			return GetNode((int)gridPosition.x, (int)gridPosition.y);
		}

		INode[] IGrid.GetAllNodesArray()
		{
			return GetAllNodesArray();
		}
	}

	public interface IGrid
	{
		float TileSize { get; }

		void SetGrid(int width, int height);
		INode GetNode(int x, int y);
		INode GetNodeByWorldPointHit(Vector2 hitPosition);
		Vector2 GetNodeWorldPosition(int x, int y);
		INode GetNode(Vector2 gridPosition);
		INode[] GetAllNodesArray();
	}
}