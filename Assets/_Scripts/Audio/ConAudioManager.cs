using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;
using System.Collections.Generic;

public class ConAudioManager : MonoBehaviour, IConfactory {

	public const int DEFAULT_STATION = 0;
	public const int EFFECTS_STATION = 1;
	public const int MUSIC_STATION = 2;

	private float defaultVolume = 0.85f;
	private float musicVolume = 0.3f;
	private float effectsVolume = 0.7f;

	public AudioLibrary Library { get; private set; }
	private Dictionary<int, AudioSource> audioStations = new Dictionary<int, AudioSource>();

	public void PlayAudio(string name, int station = 0)
	{
		PlayAudio(Library.GetAudioClip(name), station);
    }

	public void StopAudio()
	{
		foreach (KeyValuePair<int, AudioSource> pair in audioStations)
		{
			StopAudio(pair.Key);
		}
	}

	public void StopAudio(int station)
	{
		audioStations[station].Stop();
	}

	public void PlayAudio(AudioClip clip, int station = 0)
	{
		if(audioStations.ContainsKey(station))
		{
			if(audioStations[station] == null)
			{
				audioStations.Remove(station);
			}
        }
		CreateAudioStation(station);

		audioStations[station].PlayOneShot(clip);
	}

	public AudioSource CreateAudioStation(int station)
	{
		if (!audioStations.ContainsKey(station))
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			audioStations.Add(station, source);
			switch(station)
			{
				case EFFECTS_STATION:
					source.volume = effectsVolume;
					break;
				case MUSIC_STATION:
					source.volume = musicVolume;
					break;
				default:
					source.volume = defaultVolume;
					break;
            }
		}
		return audioStations[station];
	}

	public AudioSource GetAudioStation(int station, bool createIfNotAvailable = false)
	{
		if(createIfNotAvailable)
		{
			CreateAudioStation(station);
        }
		return audioStations[station];
	}

	public void ConClear()
	{
		
	}

	public void ConStruct()
	{
		Library = Resources.Load<AudioLibrary>("Libraries/AudioLibrary");
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}
}
