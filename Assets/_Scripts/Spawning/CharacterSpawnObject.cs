using UnityEngine;
using System.Collections;
using Ramses.Entities;

public class CharacterSpawnObject : EntitySpawnObject<Character>
{
	protected override void Awake()
	{
		base.Awake();
		SpawningEndedEvent -= OnSpawningEndedEvent;
		SpawningEndedEvent += OnSpawningEndedEvent;
	}

	protected override void GiveSettingsBack(Character character)
	{
		base.GiveSettingsBack(character);
		character.GetComponent<InputUser>().SetInputEnabled(true);
	}

	protected override void TakeSettingsAway(Character character)
	{
		base.TakeSettingsAway(character);
		character.GetComponent<InputUser>().SetInputEnabled(false);
	}

	private void OnSpawningEndedEvent(Character character, EntitySpawnObject<Character> spawner)
	{
		
	}

	void OnDestroy()
	{
		SpawningEndedEvent -= OnSpawningEndedEvent;
    }
	
}
