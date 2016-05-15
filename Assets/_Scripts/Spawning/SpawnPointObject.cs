using UnityEngine;
using System.Collections;
using Ramses.Entities;
public class SpawnPointObject : MonoEntity {

	public Player PlayerUsingSpawn { get; private set; }
	private CharacterSpawnObject currentSpawningObject = null;
	private ConActivePlayers conActivePlayers;
	private ConAudioManager audioManager;

	protected override void Awake()
	{
		base.Awake();
		conActivePlayers = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConActivePlayers>();
		audioManager = Ramses.Confactory.ConfactoryFinder.Instance.Give<ConAudioManager>();
    }

	public Character SpawnPlayerCharacter(Player player)
	{
		Character c = conActivePlayers.CreateCharacterForPlayer(player);
		c.gameObject.transform.position = transform.position;
		currentSpawningObject = Instantiate<GameObject>(Resources.Load<GameObject>("CharacterSpawnerObject")).GetComponent<CharacterSpawnObject>();
		currentSpawningObject.transform.position = c.transform.position;
		PlayerUsingSpawn = player;
        currentSpawningObject.Spawn(c);
		currentSpawningObject.SpawningEndedEvent -= OnAnimationEndEvent;
		currentSpawningObject.SpawningEndedEvent += OnAnimationEndEvent;
		
		audioManager.PlayAudio("HolyVoice2", ConAudioManager.EFFECTS_STATION);
		audioManager.GetAudioStation(1).volume = 0.3f;
		return c;
	}

	private void OnAnimationEndEvent(Character character, EntitySpawnObject<Character> spawnObject)
	{
		currentSpawningObject.SpawningEndedEvent -= OnAnimationEndEvent;
		PlayerUsingSpawn = null;
		currentSpawningObject = null;
	}

	protected override void OnDestroy()
	{
		if (currentSpawningObject != null)
		{
			PlayerUsingSpawn = null;
            currentSpawningObject.SpawningEndedEvent -= OnAnimationEndEvent;
		}
		base.OnDestroy();
	}
}
