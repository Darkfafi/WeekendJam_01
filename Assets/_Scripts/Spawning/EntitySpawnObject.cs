using UnityEngine;
using System.Collections;
using Ramses.Entities;

public enum EntitySpawnAnimationStates
{
	Start,
	Middle,
	End
}

public class EntitySpawnObject<T> : MonoBehaviour where T : MonoEntity{

	

	public delegate void EntityHandler(T entity, EntitySpawnObject<T> spawner);
	public event EntityHandler SpawningEndedEvent;

	[SerializeField]
	private EntitySpawnAnimationStates SettingsBackState = EntitySpawnAnimationStates.End;

	private AnimationManager animationManager;
	private T entitySpawning = null;
	private Rigidbody2D rigidBodyEntity;
	private Collider2D colliderEntity;
	private float gravityScaleObject;

	protected virtual void Awake()
	{
		animationManager = gameObject.AddComponent<AnimationManager>();
		animationManager.SetAnimationHandler(GetComponent<Animator>());
		animationManager.AnimationEndedEvent += OnAnimationEndedEvent;
	}

	public void Spawn(T entity)
	{
		transform.Translate(new Vector3(0, 0, 3));
		SpriteRenderer renderer = entity.GetComponent<SpriteRenderer>();
		renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);

		colliderEntity = entity.GetComponent<Collider2D>();
		rigidBodyEntity = entity.GetComponent<Rigidbody2D>();
		entitySpawning = entity;
		TakeSettingsAway(entitySpawning);

		animationManager.PlayAnimation("SpawnStart");
	}

	IEnumerator Animate(T entity)
	{
		if (entitySpawning != null && SettingsBackState == EntitySpawnAnimationStates.Start)
		{
			GiveSettingsBack(entitySpawning);
		}
		yield return new WaitForSeconds(0.25f);
		SpriteRenderer renderer = entity.GetComponent<SpriteRenderer>();
		animationManager.PlayAnimation("SpawnMid");
		yield return new WaitForSeconds(0.25f);
		while (renderer != null && renderer.color.a < 1)
		{
			Color c = renderer.color;
			c.a += Time.deltaTime * 0.85f;
			renderer.color = c;
			yield return null;
		}
		if (renderer != null)
		{
			renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
			yield return new WaitForSeconds(0.5f);
		}
		if (entitySpawning != null && SettingsBackState == EntitySpawnAnimationStates.Middle)
		{
			GiveSettingsBack(entitySpawning);
		}
		animationManager.PlayAnimation("SpawnEnd");
	}

	protected void OnAnimationEndedEvent(string name, float endTime)
	{
		if (name == "SpawnStart")
		{
			StartCoroutine(Animate(entitySpawning));
		}

		if (name == "SpawnEnd")
		{
			if (entitySpawning != null && SettingsBackState == EntitySpawnAnimationStates.End)
			{
				GiveSettingsBack(entitySpawning);
			}
			
			if (SpawningEndedEvent != null)
			{
				SpawningEndedEvent(entitySpawning, this);
			}
			entitySpawning = null;
			Destroy(this.gameObject);
		}
	}

	protected virtual void TakeSettingsAway(T entity)
	{
		if (colliderEntity != null)
		{
			colliderEntity.enabled = false;
		}
		if (rigidBodyEntity != null)
		{
			gravityScaleObject = rigidBodyEntity.gravityScale;
			rigidBodyEntity.gravityScale = 0;
		}
	}

	protected virtual void GiveSettingsBack(T entity)
	{
		if (colliderEntity != null)
		{
			colliderEntity.enabled = true;
		}
		if (rigidBodyEntity != null)
		{
			rigidBodyEntity.gravityScale = gravityScaleObject;
		}
	}
}

public class EntitySpawnObject : EntitySpawnObject<MonoEntity>
{

}