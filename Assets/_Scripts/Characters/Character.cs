using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour {

[SerializeField]
	private DamageHitBox hitBox;
	private BusyList busyList = new BusyList();
	private Animator animator;
	private ObjectPicker objectPicker;
	private WeaponHolder weaponHolder;
	private Rigidbody2D rigid;
    private PlatformerMovement2D platformerMovement;
	private CharacterAnimationManager animationHandler;

	private InputUser userInput;

	private float throwForceMod = 1.1f;

	public WeaponInfo CurrentWeapon {
		get { return weaponHolder.CurrentWeapon; }
	}

	protected void Awake()
	{
		rigid = gameObject.AddComponent<Rigidbody2D>();
		TouchDetector2D touch2D = gameObject.AddComponent<TouchDetector2D>();
		Collider2D colliderPlayer = GetComponent<Collider2D>();
		objectPicker = gameObject.AddComponent<ObjectPicker>();
		touch2D.SetMaskLayers(Layers.LayerMaskIgnore(new int[] { Layers.PLAYERS, Layers.OBJECTS, Layers.HITBOXES }));

		animator = gameObject.GetComponent<Animator>();
		animationHandler = gameObject.AddComponent<CharacterAnimationManager>();
		animationHandler.SetAnimationHandler(this, animator);
		animationHandler.AnimationEnded += OnAnimEnd;

		userInput = gameObject.GetComponent<InputUser>();

		userInput.InputKeyEvent += OnInputKeyEvent;
		userInput.InputAxisEvent += OnInputAxisEvent;

		rigid.gravityScale = 2;
		rigid.freezeRotation = true;
		touch2D.DistanceCheck = 0.05f;

		platformerMovement = new PlatformerMovement2D(transform, colliderPlayer, rigid, touch2D);
		platformerMovement.MoveEvent += OnMovedEvent;
		platformerMovement.LostContactWithGroundEvent += OnNoGroundEvent;
		platformerMovement.LandOnGroundEvent += OnLandGroundEvent;
		weaponHolder = new WeaponHolder(this.transform, hitBox, objectPicker);
        weaponHolder.DropWeapon(false);
    }

	void Update()
	{
		if (!busyList.InBusyAction())
		{
			animationHandler.PlayAnimation("Idle");
		}
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT) && busyList.InBusyAction(BusyConsts.ACTION_IN_AIR))
		{
			if (rigid.velocity.y < 0)
			{
				animationHandler.PlayAnimation("AirDown");
			}
			else
			{
				animationHandler.PlayAnimation("AirUp");
			}
		}
	}

	protected void OnInputKeyEvent(string name, InputAction.KeyAction keyAction)
	{
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_INPUT_DISABLE))
		{
			if (keyAction == InputAction.KeyAction.KeyDown)
			{
				if (name == InputNames.ATTACK)
				{
					Attack();
				}
				if (name == InputNames.LEFT)
				{
					Move(PlatformerMovement2D.DIR_LEFT);
				}
				else if (name == InputNames.RIGHT)
				{
					Move(PlatformerMovement2D.DIR_RIGHT);
				}

			}
			else if (keyAction == InputAction.KeyAction.OnKeyDown)
			{
				if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT))
				{
					if (name == InputNames.JUMP)
					{
						platformerMovement.Jump();
					}

					if (name == InputNames.USE)
					{
						if (CurrentWeapon == null)
						{
							weaponHolder.PickUpWeapon();
						}
						else
						{
							busyList.AddBusyAction(BusyConsts.ACTION_THROW, BusyConsts.BUSY_LAYER_COMBAT);
							animationHandler.PlayAnimation(BusyConsts.ACTION_THROW);
						}
					}
				}
			}
			else if (keyAction == InputAction.KeyAction.OnKeyUp)
			{
				if (busyList.InBusyAction(BusyConsts.ACTION_HOR_MOVING))
				{
					busyList.RemoveBusyAction(BusyConsts.ACTION_HOR_MOVING);
				}
			}
		}
	}

	private void OnInputAxisEvent(string name, float value)
	{
		if (value != 0) { 
			if (name == InputNames.LEFT)
			{
				Move(PlatformerMovement2D.DIR_LEFT);
			}
			else if (name == InputNames.RIGHT)
			{
				Move(PlatformerMovement2D.DIR_RIGHT);
			}
		}
		else if (busyList.InBusyAction(BusyConsts.ACTION_HOR_MOVING))
        {
			busyList.RemoveBusyAction(BusyConsts.ACTION_HOR_MOVING);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		DamageHitBox dmgBox = other.gameObject.GetComponent<DamageHitBox>();
		if (dmgBox != null && dmgBox != hitBox)
		{
			if (dmgBox.HitType == DamageHitBox.HitTypes.Kill)
			{
				GetKilled();
			}
			if (dmgBox.HitType == DamageHitBox.HitTypes.Ko)
			{
				GetStunned();
			}
		}
	}

	private void GetKilled()
	{
		Debug.Log("Get Killed");
		animationHandler.PlayAnimation("Death");
		busyList.AddBusyAction(BusyConsts.ACTION_DYING,BusyConsts.BUSY_LAYER_INPUT_DISABLE);
	}

	private void GetStunned()
	{
		animationHandler.PlayAnimation("KO");
		busyList.AddBusyAction(BusyConsts.ACTION_STUNNED, BusyConsts.BUSY_LAYER_INPUT_DISABLE);
		StartCoroutine(StunWaitTimer(3));
	}

	private IEnumerator StunWaitTimer(float timeInSeconds)
	{
		yield return new WaitForSeconds(timeInSeconds);
		ReleaseFromStun();
	}

	private void ReleaseFromStun()
	{
		busyList.RemoveBusyAction(BusyConsts.ACTION_STUNNED);
	}

	// Move & Attack are private because they will be triggered by AI using input user stuff.
	private void Move(int direction)
	{
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT) && platformerMovement.OnGround || !platformerMovement.OnGround)
		{
			platformerMovement.Move(direction);
		}
	}

	private void Attack()
	{
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT))
		{
			busyList.AddBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
			animationHandler.PlayAnimation(BusyConsts.ACTION_ATTACK);
			weaponHolder.SetWeaponHitbox(true);
        }
	}

	private void OnMovedEvent(float velocity)
	{
		if(busyList.InBusyAction(BusyConsts.ACTION_HOR_MOVING))
		{
			busyList.RemoveBusyAction(BusyConsts.ACTION_HOR_MOVING);
        }

		busyList.AddBusyAction(BusyConsts.ACTION_HOR_MOVING,BusyConsts.BUSY_LAYER_MOVEMENT);

		if (!busyList.InBusyAction(BusyConsts.ACTION_IN_AIR) && !busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT))
		{
			animationHandler.PlayAnimation("Walk");
		}
    }
	private void OnAnimEnd(string animationName, float animFinishedTime)
	{
		if (animationName == animationHandler.GetAnimationName(BusyConsts.ACTION_ATTACK))
		{
			if (busyList.InBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT))
			{ 
				busyList.RemoveBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
				weaponHolder.SetWeaponHitbox(false);
			}
		}
		if (animationName == animationHandler.GetAnimationName(BusyConsts.ACTION_THROW))
		{
			if (busyList.InBusyAction(BusyConsts.ACTION_THROW, BusyConsts.BUSY_LAYER_COMBAT))
			{
				busyList.RemoveBusyAction(BusyConsts.ACTION_THROW, BusyConsts.BUSY_LAYER_COMBAT);
				// If animation stopped playing after it was completed then also throw the object the character is holding
				if (animFinishedTime == 1)
				{
					Weapon objectThrown = weaponHolder.DropWeapon(true,new Vector3(0.85f * Mathf.Sign(transform.localScale.x), 0.8f, 0));
					if (objectThrown != null)
					{
						Vector3 newScale = objectThrown.transform.localScale;
						newScale = new Vector3(Mathf.Abs(newScale.x) * Mathf.Sign(transform.localScale.x), newScale.y, newScale.z);
						objectThrown.transform.localScale = newScale;

						objectThrown.RigidbodyItem.velocity += (new Vector2(Mathf.Sign(transform.localScale.x) * (throwForceMod * objectThrown.WeaponHurtVelocity), 2.1f));
					}
				}
			}
		}
	}

	private void OnNoGroundEvent()
	{
		busyList.AddBusyAction(BusyConsts.ACTION_IN_AIR, BusyConsts.BUSY_LAYER_MOVEMENT);
	}

	private void OnLandGroundEvent()
	{
		busyList.RemoveBusyAction(BusyConsts.ACTION_IN_AIR, BusyConsts.BUSY_LAYER_MOVEMENT);
	}
}