using UnityEngine;
using System.Collections;

public class PlatformerMovement2D {

	public delegate void PlatformerVelocityHandler(float velocityForce);
	public delegate void PlatformerVoidHandler();

	public event PlatformerVelocityHandler MoveEvent;
	public event PlatformerVelocityHandler JumpEvent;
    public event PlatformerVelocityHandler JumpFromGroundEvent;
	public event PlatformerVelocityHandler JumpMidAirEvent;

	public event PlatformerVoidHandler LostContactWithGroundEvent;
    public event PlatformerVoidHandler LandOnGroundEvent;
	public event PlatformerVoidHandler HitHeadEvent;

	public const int DIR_LEFT = -1;
	public const int DIR_RIGHT = 1;

	#region Options Members
	public int maxJumps = 2;
	public float movementSpeed = 5;
	public float jumpForce = 7;
	public float multiplyEachJumpWith = 0.8f;
	public bool adjustScaleDirectionToMovement = true;
	// TODO Add wallsliding and wall jumping booleans
	#endregion
	private TouchDetector2D touchDetector2D;
	private Rigidbody2D rbody2D;
	private Transform transformObject;

	public int CurrentDirection { get; private set; }
	private int jumpsBeforeGroundHit = 0; // if == 2 then the player dubble jumped
	public bool OnGround { get { return onGround; } }
	private bool onGround = false;

	public PlatformerMovement2D(Transform transformObject, Collider2D colliderObject,Rigidbody2D rigidbodyObject, TouchDetector2D touchDetector = null)
	{
		this.transformObject = transformObject;
        touchDetector2D = (touchDetector != null) ? touchDetector : transformObject.gameObject.AddComponent<TouchDetector2D>();
		touchDetector2D.MarginBordersVertical = 0.3f;
		touchDetector2D.MarginBordersHorizontal = 0.01f;
		rbody2D = rigidbodyObject;
		touchDetector2D.RunOn(colliderObject);

		touchDetector2D.HitEvent += OnHitEvent;
		touchDetector2D.HitEndEvent += OnHitEndEvent;
	}
	
	public void Move(int horizontalDirection, float speedMultiplier = 1)
	{
		if(horizontalDirection > 1)
		{
			horizontalDirection = 1;
        }
		else if(horizontalDirection < -1)
		{
			horizontalDirection = -1;
        }
		if (touchDetector2D.GetHitInfoFromSide(new Vector2(horizontalDirection, 0)) == null) //Don't move if something is blocking your path
		{
			float calculatedSpeed = (movementSpeed * speedMultiplier);
            transformObject.Translate(new Vector2(horizontalDirection * calculatedSpeed, 0) * Time.deltaTime);
			CurrentDirection = horizontalDirection;
			if(MoveEvent != null)
			{
				MoveEvent(calculatedSpeed);
            }
		}
		if (adjustScaleDirectionToMovement)
		{
			transformObject.localScale = new Vector3(Mathf.Abs(transformObject.localScale.x) * horizontalDirection, transformObject.localScale.y, transformObject.localScale.z);
        }
    }

	public void Jump(float jumpForceMultiplier = 1)
	{
		if(touchDetector2D.GetHitInfoFromSide(Vector2.up) == null){
			if (jumpsBeforeGroundHit < maxJumps)
			{
				float calculatedJumpForce = (jumpForce * (Mathf.Pow(multiplyEachJumpWith, jumpsBeforeGroundHit))) * jumpForceMultiplier;
                rbody2D.velocity = new Vector2(0, calculatedJumpForce);
				if(JumpEvent != null)
				{
					JumpEvent(calculatedJumpForce);
                }
				if (onGround)
				{
					//Ground jump
					if(JumpFromGroundEvent != null)
					{
						JumpFromGroundEvent(calculatedJumpForce);
                    }
                }
				else
				{
					//Air jump 
					if(JumpMidAirEvent != null)
					{
						JumpMidAirEvent(calculatedJumpForce);
                    }
                }
				jumpsBeforeGroundHit++;
			}
		}
	}

	private void OnHitEvent(Vector2 side, Collider2D collider)
	{
		if(side == Vector2.down && !onGround)
		{
			onGround = true;
			jumpsBeforeGroundHit = 0;
			if (LandOnGroundEvent != null)
			{
				LandOnGroundEvent();
            }
        }
		if(side == Vector2.up)
		{
			rbody2D.velocity = new Vector2(rbody2D.velocity.x, 0);
			if(HitHeadEvent != null)
			{
				HitHeadEvent();
            }
		}
	}
	private void OnHitEndEvent(Vector2 side, Collider2D collider)
	{
		if (side == Vector2.down)
		{
			onGround = false;
			if(jumpsBeforeGroundHit == 0)
			{
				jumpsBeforeGroundHit = 1;
			}
			if (LostContactWithGroundEvent != null)
			{
				LostContactWithGroundEvent();
            }
		}
	}
}
