using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ramses.Confactory;
using System;

public class MovementAIPart : IAIPart
{
	public AIGridNode[] WaypointsLeft { get { return currentFocusWaypoints.ToArray(); } }
	private List<AIGridNode> currentFocusWaypoints = new List<AIGridNode>(); // Its current path
	private AIInputSubmitter inputSubmitter = null;
	private ConAiGridManager aiGridManager;
	private PlatformerMovement2D platformerMovement2D;

	private List<string> usingKeys = new List<string>();

	private AIGridNode jumpStartedNode = null;

	public MovementAIPart(AIInputSubmitter submitter, PlatformerMovement2D platformerMovement2D)
	{
		aiGridManager = ConfactoryFinder.Instance.Give<ConAiGridManager>();
		this.inputSubmitter = submitter;
		this.platformerMovement2D = platformerMovement2D;
    }

	public void Move(AIGridNode[] waypoints, bool addToCurrentAssignment = false)
	{
		if (!addToCurrentAssignment)
		{
			currentFocusWaypoints.Clear();
		}
		currentFocusWaypoints.AddRange(waypoints);
    }

	public void UpdatePart()
	{
		if (currentFocusWaypoints.Count > 0)
		{
			if (currentFocusWaypoints[0].GetWorldPosition().DistanceTo(platformerMovement2D.Position) >= (currentFocusWaypoints[0].Size * 0.5f))
			{
				MoveTo(currentFocusWaypoints[0]);
			}
			else
			{
				currentFocusWaypoints.RemoveAt(0);
			}
		}
		else
		{
			inputSubmitter.ReleaseKey(InputNames.LEFT);
			inputSubmitter.ReleaseKey(InputNames.RIGHT);
		}
	}

	private void MoveTo(AIGridNode positionNode)
	{
		Vector2 position = positionNode.GetWorldPosition();
		AIGridNode currentNode = aiGridManager.AIGrid.GetNodeByWorldPointHit(platformerMovement2D.Position + (Vector2.up * (positionNode.Size * 0.5f)));
		if (positionNode.Data.IsGround())
		{
			jumpStartedNode = null;
		}

        if (position.x < platformerMovement2D.Position.x && position.YFlatten().DistanceTo(platformerMovement2D.Position.YFlatten()) > 0.04f)
		{
			inputSubmitter.DoInputAction(InputNames.LEFT);
        }
		else if(position.x > platformerMovement2D.Position.x && position.YFlatten().DistanceTo(platformerMovement2D.Position.YFlatten()) > 0.04f)
		{
			inputSubmitter.DoInputAction(InputNames.RIGHT);
		}
		else
		{
			inputSubmitter.ReleaseKey(InputNames.LEFT);
			inputSubmitter.ReleaseKey(InputNames.RIGHT);
		}

		if(positionNode.PositionY > currentNode.PositionY)
		{
			
			if (platformerMovement2D.TimesJumpedBeforeGroundHit == 0)
			{
				inputSubmitter.ReleaseKey(InputNames.JUMP);
				if (currentNode.Data.IsGround())
				{
					jumpStartedNode = currentNode;
                }
			}
			if (JumpAgainCheck(jumpStartedNode, currentNode, positionNode))
			{
				inputSubmitter.ReleaseKey(InputNames.JUMP);
				inputSubmitter.DoInputAction(InputNames.JUMP);
			}
		}
		else
		{
			inputSubmitter.ReleaseKey(InputNames.JUMP);
		}
	}

	private bool JumpAgainCheck(AIGridNode startJumpNode, AIGridNode currentNode, AIGridNode targetNode)
	{
		if (platformerMovement2D.TimesJumpedBeforeGroundHit == 0)
			return true;

		if (jumpStartedNode != null)
		{
			float currentNodeCost = (currentNode.PositionY - startJumpNode.PositionY) * 2;
			float currentJumpMaxCostReach = AStarPlatforming.CalculateMaxJumpCost(platformerMovement2D.GetPotentialMaxJumpHeight(platformerMovement2D.TimesJumpedBeforeGroundHit), currentNode.Size);
			Debug.Log(currentNodeCost + "jump check" + currentJumpMaxCostReach);
			if(currentNodeCost >= currentJumpMaxCostReach)
			{
				return true;
			}
		}else if(currentNode.Data.IsGround())
		{
			jumpStartedNode = currentNode;
        }

		return false;
	}
}
