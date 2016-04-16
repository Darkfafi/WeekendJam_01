using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadLobby()
	{
		SceneManager.LoadScene("Lobby");
	}
}
