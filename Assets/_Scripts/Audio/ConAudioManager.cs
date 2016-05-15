using UnityEngine;
using System.Collections;
using Ramses.Confactory;
using System;
using System.Collections.Generic;

public class ConAudioManager : MonoBehaviour, IConfactory {

	public const int DEFAULT_STATION = 0;
	public const int EFFECTS_STATION = 1;
	public const int MUSIC_STATION = 2;

	private const float startDefaultVolume = 1.2f;
	private const float startMusicVolume = 0.3f;
	private const float startEffectsVolume = 0.7f;

	private const float startDefaultPitch = 1.1f;
	private const float startMusicPitch = 1.15f;
	private const float startEffectsPitch = 1;

	public AudioLibrary Library { get; private set; }
	private Dictionary<int, AudioSource> audioStations = new Dictionary<int, AudioSource>();
	private Dictionary<int, StationSettings> audioStationsSettings = new Dictionary<int, StationSettings>(); //1.15

	public void PlayAudio(string name, int station = 0, float volumeScale = 1)
	{
		PlayAudio(Library.GetAudioClip(name), station, volumeScale);
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
		if (audioStations.ContainsKey(station))
		{
			audioStations[station].Stop();
		}
	}

	public void PlayAudio(AudioClip clip, int station = 0, float volumeScale = 1)
	{
		if(audioStations.ContainsKey(station))
		{
			if(audioStations[station] == null)
			{
				audioStations.Remove(station);
			}
        }
		GetAudioStation(station).PlayOneShot(clip, volumeScale);
	}

	public AudioSource GetAudioStation(int station)
	{
		if (!audioStations.ContainsKey(station))
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			audioStations.Add(station, source);
			SetSettingsForAudioStation(station);
		}
		return audioStations[station];
	}

	private StationSettings GetStationSettings(int station)
	{
		if (!audioStationsSettings.ContainsKey(station))
		{
			audioStationsSettings.Add(station, new StationSettings());
		}
		return audioStationsSettings[station];
	}

	private void SetSettingsForAudioStation(int station)
	{
		StationSettings settings = GetStationSettings(station);
		AudioSource aStation = GetAudioStation(station);
		aStation.pitch = settings.Pitch;
		aStation.volume = settings.Volume;
	}

	public float GetAudioStationVolume(int station)
	{
		return GetStationSettings(station).Volume;
	}

	public void SetAudioStationVolume(int station, float volume)
	{
		GetStationSettings(station).Volume = volume;
		SetSettingsForAudioStation(station);
    }

	public float GetAudioStationPitch(int station)
	{
		return GetStationSettings(station).Pitch;
	}

	public void SetAudioStationPitch(int station, float pitch)
	{
		GetStationSettings(station).Pitch = pitch;
		SetSettingsForAudioStation(station);
    }

	public void ConClear()
	{
		
	}

	public void ConStruct()
	{
		Library = Resources.Load<AudioLibrary>("Libraries/AudioLibrary");

		SetAudioStationVolume(DEFAULT_STATION, startDefaultVolume);
		SetAudioStationVolume(EFFECTS_STATION, startEffectsVolume);
		SetAudioStationVolume(MUSIC_STATION, startMusicVolume);
		SetAudioStationPitch(DEFAULT_STATION, startDefaultPitch);
		SetAudioStationPitch(EFFECTS_STATION, startEffectsPitch);
		SetAudioStationPitch(MUSIC_STATION, startMusicPitch);
	}

	public void OnSceneSwitch(int newSceneIndex)
	{
		
	}

	private class StationSettings
	{
		public float Pitch = 1;
		public float Volume = 1;
	}
}
