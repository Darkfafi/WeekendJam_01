using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

[SerializeField]
	private DamageHitBox hitBox;
	private BusyList busyList = new BusyList();
	private Animator animator;
	private ObjectPicker objectPicker;
	private WeaponHolder weaponHolder;
	private Rigidbody2D rigid;
    private PlatformerMovement2D platformerMovement;
	private PlayerAnimationHandler animationHandler;

	private UserInput userInput;

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
		animationHandler = new PlayerAnimationHandler(this, animator);

		userInput = gameObject.GetComponent<UserInput>();

		if(userInput != null)
		{
			userInput.KeyEvent += OnKeyEvent;
			userInput.KeyDownEvent += OnKeyDownEvent;
			userInput.KeyUpEvent += OnKeyUpEvent;
		}

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
		CheckAnimations();
		if (!busyList.InBusyAction())
		{
			animationHandler.PlayAnimation("Idle");
		}
	}

	protected void OnKeyEvent(KeyBindings bindings, KeyCode key)
	{
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_INPUT_DISABLE))
		{
			if (key == bindings.Right)
			{
				Move(PlatformerMovement2D.DIR_RIGHT);
			}
			else if (key == bindings.Left)
			{
				Move(PlatformerMovement2D.DIR_LEFT);
			}
			if (key == bindings.Attack)
			{
				Attack();
			}
		}
	}

	private void OnKeyDownEvent(KeyBindings bindings, KeyCode key)
	{
		if(!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT) 
			&& !busyList.InBusyAction(BusyConsts.BUSY_LAYER_INPUT_DISABLE))
		{
			if (key == bindings.Jump)
			{
				platformerMovement.Jump();
			}

			if (key == bindings.Use)
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

	private void OnKeyUpEvent(KeyBindings bindings, KeyCode key)
	{
		if (busyList.InBusyAction(BusyConsts.ACTION_HOR_MOVING))
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
		Debug.Log("Get Stunned");
		animationHandler.PlayAnimation("KO");
		busyList.AddBusyAction(BusyConsts.ACTION_STUNNED, BusyConsts.BUSY_LAYER_INPUT_DISABLE);
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

		if (!busyList.InBusyAction(BusyConsts.ACTION_IN_AIR))
		{
			animationHandler.PlayAnimation("Walk");
		}
    }
	private void CheckAnimations()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationHandler.GetAnimationName("Attack")))
		{
			if (busyList.InBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT))
			{
				busyList.RemoveBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
			}
		}
		if(!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT) && busyList.InBusyAction(BusyConsts.ACTION_IN_AIR))
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
	private void OnNoGroundEvent()
	{
		busyList.AddBusyAction(BusyConsts.ACTION_IN_AIR, BusyConsts.BUSY_LAYER_MOVEMENT);
	}
	private void OnLandGroundEvent()
	{
		busyList.RemoveBusyAction(BusyConsts.ACTION_IN_AIR, BusyConsts.BUSY_LAYER_MOVEMENT);
	}
}