using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathController : MonoBehaviour {

	void Update () {
		
	}

	public void ReplayGame()
	{
		Debug.Log("Restart");
		SceneManager.LoadScene(0);
	}

	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
