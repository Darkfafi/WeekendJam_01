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

	public WeaponInfo CurrentWeapon {
		get { return weaponHolder.CurrentWeapon; }
	}

	protected void Awake()
	{
		rigid = gameObject.AddComponent<Rigidbody2D>();
		TouchDetector2D touch2D = gameObject.AddComponent<TouchDetector2D>();
		Collider2D colliderPlayer = GetComponent<Collider2D>();
		objectPicker = gameObject.AddComponent<ObjectPicker>();
		touch2D.SetMaskLayers(Layers.LayerMaskIgnore(new int[] { Layers.PLAYERS, Layers.OBJECTS }));

		animator = gameObject.GetComponent<Animator>();
		animationHandler = gameObject.AddComponent<CharacterAnimationManager>();
		animationHandler.SetAnimationHandler(this, animator);
		animationHandler.AnimationEnded += OnAnimEnd;

		userInput = gameObject.GetComponent<InputUser>();

		userInput.InputEvent += OnInputEvent;

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

	protected void OnInputEvent(InputAction action)
	{
		if(action.Type == InputItem.InputType.KeyCode)
		{
			if(action.Value == InputAction.VALUE_KEY_DOWN)
			{
				if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_INPUT_DISABLE))
				{
					if (action.Name == InputNames.ATTACK)
					{
						Attack();
					}
					if (action.Name == InputNames.LEFT)
					{
						Move(PlatformerMovement2D.DIR_LEFT);
					}
					else if (action.Name == InputNames.RIGHT)
					{
						Move(PlatformerMovement2D.DIR_RIGHT);
					}
				}
			}
			else if(action.Value == InputAction.VALUE_ON_KEY_DOWN)
			{
				if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT)
					&& !busyList.InBusyAction(BusyConsts.BUSY_LAYER_INPUT_DISABLE))
				{
					if (action.Name == InputNames.JUMP)
					{
						platformerMovement.Jump();
					}

					if (action.Name == InputNames.USE)
					{
						if (CurrentWeapon == null)
						{
							weaponHolder.PickUpWeapon();
						}
						else
						{
							weaponHolder.DropWeapon();
						}
					}
				}
			}
			else if(action.Value == InputAction.VALUE_ON_KEY_UP)
			{
				if (busyList.InBusyAction(BusyConsts.ACTION_HOR_MOVING))
				{
					busyList.RemoveBusyAction(BusyConsts.ACTION_HOR_MOVING);
				}
			}
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
	public void Move(int direction)
	{
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT) && platformerMovement.OnGround || !platformerMovement.OnGround)
		{
			platformerMovement.Move(direction);
		}
	}
	public void Attack()
	{
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT))
		{
			busyList.AddBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
			animationHandler.PlayAnimation(BusyConsts.ACTION_ATTACK);
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
	private void OnAnimEnd(string animationName)
	{
		if (animationName == animationHandler.GetAnimationName("Attack"))
		{
			if (busyList.InBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT))
			{ 
				busyList.RemoveBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
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