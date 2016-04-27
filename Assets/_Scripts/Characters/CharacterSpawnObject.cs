using UnityEngine;
using System.Collections;

public class CharacterSpawnObject : MonoBehaviour {

	public delegate void CharacterHandler(Character character, CharacterSpawnObject spawner);
	public event CharacterHandler SpawningEndedEvent;

	private AnimationManager animationManager;
	private Character characterSpawning = null;
	private float gravityScaleObject;

	private void Awake()
	{
		animationManager = gameObject.AddComponent<AnimationManager>();
        animationManager.SetAnimationHandler(GetComponent<Animator>());
		animationManager.AnimationEndedEvent += OnAnimationEndedEvent;
		transform.Translate(new Vector3(0, 0, 1));
    }

	public void SpawnCharacter(Character character)
	{
		SpriteRenderer renderer = character.GetComponent<SpriteRenderer>();
		renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
		character.transform.position = transform.position;
		character.GetComponent<InputUser>().SetInputEnabled(false);
		character.CharacterCollider.enabled = false;
		gravityScaleObject = character.CharacterRigidbody2D.gravityScale;
		character.CharacterRigidbody2D.gravityScale = 0;

		characterSpawning = character;
		animationManager.PlayAnimation("SpawnStart");
	}

	IEnumerator Animate(Character character)
	{
		yield return new WaitForSeconds(0.25f);
		SpriteRenderer renderer = character.GetComponent<SpriteRenderer>();
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
		animationManager.PlayAnimation("SpawnEnd");
	}

	private void OnAnimationEndedEvent(string name, float endTime)
	{
		if(name == "SpawnStart")
		{
			StartCoroutine(Animate(characterSpawning));
		}

		if(name == "SpawnEnd")
		{
			if (characterSpawning != null)
			{
				characterSpawning.GetComponent<InputUser>().SetInputEnabled(true);
				characterSpawning.CharacterCollider.enabled = true;
				characterSpawning.CharacterRigidbody2D.gravityScale = gravityScaleObject;
			}

            if (SpawningEndedEvent != null)
			{
				SpawningEndedEvent(characterSpawning, this);
            }
			characterSpawning = null;
			Destroy(this.gameObject);
		}
	}
}
