using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioLibrary : ScriptableObject {

	[SerializeField]
	private AudioInfo[] allAudio;

	private Dictionary<string, AudioClip> audioDic = null;

	public AudioClip GetAudioClip(string nameAudio)
	{
		if(audioDic == null)
		{
			audioDic = new Dictionary<string, AudioClip>();
			foreach(AudioInfo info in allAudio)
			{
				audioDic.Add(info.Name, info.AudioClip);
			}
		}

		return audioDic[nameAudio];
    }

	[Serializable]
	private class AudioInfo
	{
		[SerializeField]
		public string Name;
		[SerializeField]
		public AudioClip AudioClip;
	}
}
