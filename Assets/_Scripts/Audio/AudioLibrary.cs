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
				if (!audioDic.ContainsKey(info.Name))
				{
					audioDic.Add(info.Name, info.AudioClip);
				}
				else
				{
					Debug.LogError("Cannot have multiple audioclips in AudioLibrary with the name: '" + info.Name+"'!");
				}
			}
		}
		if (audioDic.ContainsKey(nameAudio))
		{
			return audioDic[nameAudio];
		}else
		{
			Debug.LogError("Could not find audio with name: " + nameAudio + "! Be sure the audioLibrary item you desire has the name given!");
			return null;
		}
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
