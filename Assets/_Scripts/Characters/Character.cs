using UnityEngine;
using System.Collections;
using System;
using Ramses.Entities;
using Ramses.Confactory;

public class Character : MonoEntity {

	public delegate void CharacterHandler(Character character);
	public delegate void CharacterDualHandler(Character effected, Character effecter);

	public event CharacterDualHandler CharacterGotKilledEvent;
	public event CharacterHandler CharacterDestroyEvent; // Character script gets destroyed on corpse spawn and complete destruction of game

	public Collider2D CharacterCollider { get; private set; }
	public Rigidbody2D CharacterRigidbody2D { get; private set; }
	public Vector2 SizeCharacter { get; private set; }

	public bool IsAlive { get; private set; }

    public WeaponInfo CurrentWeapon
	{
		get { return weaponHolder.CurrentWeapon; }
	}

	[SerializeField]
	private DamageHitBox hitBox;

	private BusyList busyList = new BusyList();
	private Animator animator;
	private ObjectPicker objectPicker;
	private WeaponHolder weaponHolder;
    private PlatformerMovement2D platformerMovement;
	private CharacterAnimationManager animationHandler;
	private InputUser userInput;
	private TouchDetector2D touch2D;

	private ConAudioManager audioManager;

	// Options
	[SerializeField] private float throwForceMod = 1.1f;
	[SerializeField] private float movementSpeed = 6f;
	[SerializeField] private float jumpForce = 8f;

	protected override void Awake()
	{
		base.Awake();
		IsAlive = true;

		audioManager = ConfactoryFinder.Instance.Give<ConAudioManager>();

		CharacterRigidbody2D = gameObject.AddComponent<Rigidbody2D>();
		touch2D = gameObject.AddComponent<TouchDetector2D>();
		CharacterCollider = GetComponent<Collider2D>();
		objectPicker = gameObject.AddComponent<ObjectPicker>();
		touch2D.SetMaskLayers(Layers.LayerMaskSeeOnly(new int[] { Layers.DEFAULT }));

		animator = gameObject.GetComponent<Animator>();
		animationHandler = gameObject.AddComponent<CharacterAnimationManager>();
		animationHandler.SetAnimationHandler(this, animator);
		animationHandler.AnimationEndedEvent += OnAnimEnd;

		userInput = gameObject.GetComponent<InputUser>();

		userInput.InputKeyEvent += OnInputKeyEvent;
		userInput.InputAxisEvent += OnInputAxisEvent;

		hitBox.SetOwner(this);

		CharacterRigidbody2D.gravityScale = 2;
		CharacterRigidbody2D.freezeRotation = true;
		SizeCharacter = CharacterCollider.bounds.size;
        touch2D.DistanceCheck = 0.05f;

		platformerMovement = new PlatformerMovement2D(transform, CharacterCollider, CharacterRigidbody2D, touch2D);
		platformerMovement.MovementSpeed = movementSpeed;
		platformerMovement.JumpForce = jumpForce;

		platformerMovement.MoveEvent += OnMovedEvent;
		platformerMovement.LostContactWithGroundEvent += OnNoGroundEvent;
		platformerMovement.LandOnGroundEvent += OnLandGroundEvent;
		platformerMovement.JumpEvent += OnJumpEvent;

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
			if (CharacterRigidbody2D.velocity.y < 0)
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

					if (name == InputNames.GRAB_THROW)
					{
						if (CurrentWeapon == null)
						{
							if (weaponHolder.PickUpWeapon())
							{
								audioManager.PlayAudio("Grab", ConAudioManager.EFFECTS_STATION, 1.5f);
							}
						}
						else
						{
							busyList.AddBusyAction(BusyConsts.ACTION_THROW, BusyConsts.BUSY_LAYER_COMBAT);
							animationHandler.PlayAnimation(BusyConsts.ACTION_THROW);
							audioManager.PlayAudio("ThrowEffect", ConAudioManager.EFFECTS_STATION, 0.5f);
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
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_INPUT_DISABLE))
		{
			if (value != 0)
			{
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
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		DamageHitBox dmgBox = other.gameObject.GetComponent<DamageHitBox>();
		if (dmgBox != null && dmgBox != hitBox)
		{
			if (dmgBox.HitType == DamageHitBox.HitTypes.Kill)
			{
				audioManager.PlayAudio("Stab", ConAudioManager.EFFECTS_STATION);
				GetKilled(dmgBox.Owner);
			}
			if (dmgBox.HitType == DamageHitBox.HitTypes.Ko)
			{
				audioManager.PlayAudio("PunchHit", ConAudioManager.EFFECTS_STATION,0.35f);
				GetStunned(dmgBox.Owner);
			}
		}
	}

	private void GetKilled(Character killer)
	{
		IsAlive = false;

		weaponHolder.Disarm(Vector2.up, 6);

		Destroy(CharacterCollider);
		Destroy(touch2D);

		if(CharacterGotKilledEvent != null)
		{
			CharacterGotKilledEvent(this, killer);
		}

		animationHandler.PlayAnimation("Death");
		busyList.ClearBusyList();
		busyList.AddBusyAction(BusyConsts.ACTION_DYING,BusyConsts.BUSY_LAYER_INPUT_DISABLE);
	}

	private void GetStunned(Character stunner)
	{
		animationHandler.PlayAnimation("KO");
		busyList.ClearBusyList();
		CharacterCollider.enabled = false;

		weaponHolder.Disarm(Vector2.up, 6);

		busyList.AddBusyAction(BusyConsts.ACTION_STUNNED, BusyConsts.BUSY_LAYER_INPUT_DISABLE);
		StartCoroutine(StunWaitTimer(1.5f));
	}

	private IEnumerator StunWaitTimer(float timeInSeconds)
	{
		yield return new WaitForSeconds(timeInSeconds);
		ReleaseFromStun();
	}

	private void ReleaseFromStun()
	{
		CharacterCollider.enabled = true;
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
		if (!busyList.InBusyAction(BusyConsts.BUSY_LAYER_COMBAT) && !animationHandler.AnimatorInAnimation(BusyConsts.ACTION_ATTACK))
		{
			busyList.AddBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
			animationHandler.PlayAnimation(BusyConsts.ACTION_ATTACK);
			weaponHolder.SetWeaponHitbox(true);
			if(weaponHolder.CurrentWeapon != null &&
				weaponHolder.CurrentWeapon.WeaponAttackType == DamageHitBox.HitTypes.Kill)
			{
				audioManager.PlayAudio("AirForce3", ConAudioManager.EFFECTS_STATION, 2);
			}
			else
			{
				audioManager.PlayAudio("AirForce4", ConAudioManager.EFFECTS_STATION, 2);
			}
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
		if (animationHandler.PlayedAnimationNameCheck(animationName, BusyConsts.ACTION_ATTACK))
		{
			if (busyList.InBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT))
			{ 
				busyList.RemoveBusyAction(BusyConsts.ACTION_ATTACK, BusyConsts.BUSY_LAYER_COMBAT);
				weaponHolder.SetWeaponHitbox(false);
			}
		}
		if (animationHandler.PlayedAnimationNameCheck(animationName, BusyConsts.ACTION_THROW))//animationName == animationHandler.GetAnimationName(BusyConsts.ACTION_THROW))
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
						objectThrown.SetHitboxItem(true, this);
                    }
				}
			}
		}
		if (animationHandler.PlayedAnimationNameCheck(animationName, "Death"))
		{
			Destroy(this);
		}
	}

	private void FootstepTriggered()
	{
		audioManager.PlayAudio("Step", ConAudioManager.EFFECTS_STATION, 2);
	}

	private void OnNoGroundEvent()
	{
		busyList.AddBusyAction(BusyConsts.ACTION_IN_AIR, BusyConsts.BUSY_LAYER_MOVEMENT);
		audioManager.PlayAudio("AirForce4", ConAudioManager.EFFECTS_STATION, 0.7f);
	}

	private void OnJumpEvent(float force)
	{
		audioManager.PlayAudio("AirForceSound2", ConAudioManager.EFFECTS_STATION, 1.55f);
	}

	private void OnLandGroundEvent()
	{
		busyList.RemoveBusyAction(BusyConsts.ACTION_IN_AIR, BusyConsts.BUSY_LAYER_MOVEMENT);
	}

	protected override void OnDestroy()
	{
		RemoveEventListeners();

		if (CharacterDestroyEvent != null)
		{
			CharacterDestroyEvent(this);
		}

		Destroy(userInput);
		Destroy(animationHandler);
		Destroy(objectPicker);
		Destroy(animator);

		base.OnDestroy();
	}

	private void RemoveEventListeners()
	{
		if (platformerMovement != null)
		{
			platformerMovement.MoveEvent -= OnMovedEvent;
			platformerMovement.LostContactWithGroundEvent -= OnNoGroundEvent;
			platformerMovement.LandOnGroundEvent -= OnLandGroundEvent;
			platformerMovement.JumpEvent -= OnJumpEvent;
		}
		if (animationHandler != null)
		{
			animationHandler.AnimationEndedEvent += OnAnimEnd;
		}
		if (userInput != null)
		{
			userInput.InputKeyEvent += OnInputKeyEvent;
			userInput.InputAxisEvent += OnInputAxisEvent;
		}
	}
}