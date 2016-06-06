using UnityEngine;
using System.Collections;
using System;

namespace Ramses.Grid
{
	public abstract class Node<T> : INode where T : class, INodeData
	{
		public IGrid Grid { get; private set; }
		public float Size { get; private set; }

		// Position indicates Grid position 
		public Vector2 Position { get; private set; }
		public int PositionX { get { return (int)Position.x; } }
		public int PositionY { get { return (int)Position.y; } }

		public virtual void SetNode(IGrid grid, int xPosition, int yPosition, float size)
		{
			Position = new Vector2(xPosition, yPosition);
			Grid = grid;
			Size = size;
		}

		public virtual T DataCheck()
		{
			Debug.LogError("DataCheck not correctly implemented. Do not call the base of this method");
			return null;
		}

		public Vector2 GetWorldPosition()
		{
			return new Vector2(Size * PositionX, Size * PositionY);
		}

		public bool WorldPositionInBounds(Vector2 position)
		{
			Vector2 ownPos = GetWorldPosition();
			if(ownPos.x - (Size * 0.5f) < position.x && ownPos.x + (Size * 0.5f) > position.x
			&& ownPos.y - (Size * 0.5f) < position.y && ownPos.y + (Size * 0.5f) > position.y)
			{
				return true;
			}
			return false;
		}

		INodeData INode.DataCheck()
		{
			return ((T)DataCheck() as T);
		}
	}

	public interface INode
	{
		float Size { get; }
		Vector2 Position { get; }
		int PositionX { get; }
		int PositionY { get; }
		void SetNode(IGrid grid, int xPosition, int yPosition, float size);
        INodeData DataCheck();

		// World Checks
		Vector2 GetWorldPosition();
		bool WorldPositionInBounds(Vector2 position);
	}

	public interface INodeData
	{

	}
}
