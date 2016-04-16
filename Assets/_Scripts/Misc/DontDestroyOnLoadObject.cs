using UnityEngine;
using System.Collections;

public class DontDestroyOnLoadObject : MonoBehaviour {

	// Use this for initialization
	private static DontDestroyOnLoadObject instance;
	void Awake () {
		if (instance)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
