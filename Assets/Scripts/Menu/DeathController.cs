using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathController : MonoBehaviour {

	public int lastKillRatio;
	public int recordKillRatio;
	public bool isNewRecord;
	private GUIStyle style;
	private string killRatio;
	private Vector2 killRatioSizeText;
	private float nativeSize = 800.0f;

	void Start() {
		style = new GUIStyle();
		style.fontSize = (int)(10.0f * ((float)Screen.width / nativeSize));
		Cursor.visible = true;
		isNewRecord = false;
		lastKillRatio = PlayerPrefs.GetInt("LastKillRatio", 0);
		recordKillRatio = PlayerPrefs.GetInt("RecordKillRatio", 0);

		if (lastKillRatio > recordKillRatio) {
			isNewRecord = true;
			PlayerPrefs.SetInt("RecordKillRatio", lastKillRatio);
		}

		killRatio = string.Format("Enemies killed: {0}\nHighscore kills: {1}", lastKillRatio, recordKillRatio);
		if (isNewRecord) { killRatio += "\nNEW HIGHSCORE!"; }

		killRatioSizeText = style.CalcSize(new GUIContent(killRatio));
	}

	void OnGUI() {
		ShowLabel(Screen.width / 2 - killRatioSizeText.x / 2, Screen.height / 3 * 2, killRatio, killRatioSizeText);
	}

	void ShowLabel(float posX, float posY, string text, Vector2 textSize, bool alignToRight = false) {
		if (alignToRight) { posX -= textSize.x; }

		if (posX + textSize.x > Screen.width) { posX = Screen.width - textSize.x; }
		if (posY + textSize.y > Screen.height) { posY = Screen.height - textSize.y; }
		if (posX < 0) { posX = 0; }
		if (posY < 0) { posY = 0; }

		GUI.Label(new Rect(posX, posY, textSize.x, textSize.y), text, style);
	}

	public void ReplayGame() {
		Debug.Log("Restart");
		SceneManager.LoadScene(0);
	}

	public void QuitGame() {
		Debug.Log("Quit");
		Application.Quit();
	}
}
