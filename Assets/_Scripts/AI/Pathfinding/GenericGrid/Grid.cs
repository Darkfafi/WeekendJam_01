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
		public int Width;
		public int Height;

		public Vector2 Offset { get; private set; }

		public Grid(float tileSize, Vector2? offset = null)
		{
			this.TileSize = tileSize;
			if(offset.HasValue)
			{
				Offset = offset.Value;
			}
			else
			{
				offset = new Vector2();
			}
		}

		public void SetGrid(int width, int height)
		{
			Width = width;
			Height = height;
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

		public void ClearGrid()
		{
			Width = 0;
			Height = 0;
			allNodes_YX_Oder.Clear();
			TileSize = 0;
		}

		public T GetNode(int x, int y)
		{
			T nodeToReturn = null;

			if (x >= 0 && y >= 0 && y < allNodes_YX_Oder.Count && x < allNodes_YX_Oder[y].Count)
			{
				if (allNodes_YX_Oder[y] != null && allNodes_YX_Oder[y][x] != null)
				{
					nodeToReturn = allNodes_YX_Oder[y][x];
				}
			}

			return nodeToReturn;
		}

		public T GetNodeByWorldPointHit(Vector2 hitPosition)
		{
			float gridWidth = TileSize * Width;
			float gridHeight = TileSize * Height;
			Vector2 hitNodePos = new Vector2(Mathf.FloorToInt(((hitPosition.x + TileSize / 2) / gridWidth) * Width),
				Mathf.FloorToInt(((hitPosition.y + TileSize / 2) / gridHeight) * Height));
			
			return GetNode(hitNodePos);
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
		Vector2 Offset { get; }

		void SetGrid(int width, int height);
		INode GetNode(int x, int y);
		INode GetNodeByWorldPointHit(Vector2 hitPosition);
		Vector2 GetNodeWorldPosition(int x, int y);
		INode GetNode(Vector2 gridPosition);
		INode[] GetAllNodesArray();
	}
}