using UnityEngine;
using System.Collections;
using Ramses.Confactory;

public class CharacterAI : MonoBehaviour {

	private MovementAIPart movementAIPart;
	private AIInputSubmitter inputSubmitter;
	private Character character;
	private ConAiGridManager gridManager;

	// Use this for initialization
	private void Start ()
	{
		gridManager = ConfactoryFinder.Instance.Give<ConAiGridManager>();
		character = gameObject.GetComponent<Character>();
		inputSubmitter = new AIInputSubmitter(character.GetComponent<InputUser>());
		movementAIPart = new MovementAIPart(inputSubmitter, character.PlatformerMovement);
    }
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			AIGridNode[] waypoints = AStarPlatforming.Search(gridManager.AIGrid, gridManager.AIGrid.GetNodeByWorldPointHit(transform.position).Position, 
				gridManager.AIGrid.GetNodeByWorldPointHit(Camera.main.ScreenToWorldPoint(Input.mousePosition)).Position, character.PlatformerMovement);
			movementAIPart.Move(waypoints);
		}
		gridManager.SetDebugWaypoints(movementAIPart.WaypointsLeft);
		movementAIPart.UpdatePart();
    }
}
