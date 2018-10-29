using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	private Version programVersion = new Version("0.1.0");
	private GameObject player;
	private GameObject [] enemies;

	bool showDebugInfo = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			showDebugInfo = showDebugInfo ? false : true;
		}
	}
	
	void OnGUI()
	{
		ShowLabel(Screen.width - 10, Screen.height - 10, string.Format("Health: {0}", player.GetComponent<HealthSystem>().currentHealth)); // życie gracza
		ShowLabel(Screen.width - 10, 10, "Copyright 2018 Rafał Bukowski\nv." + programVersion, true);
		ShowLabel(10, 10, GetDebugInfo());
	}

	void ShowLabel(float posX, float posY, string text, bool alignToRight = false)
	{
		var calcualtedSize = GUI.skin.label.CalcSize(new GUIContent(text));

		if (alignToRight) { posX -= calcualtedSize.x; }

		if (posX + calcualtedSize.x > Screen.width) { posX = Screen.width - calcualtedSize.x; }
		if (posY + calcualtedSize.y > Screen.height) { posY = Screen.height - calcualtedSize.y; }
		if (posX < 0) { posX = 0; }
		if (posY < 0) { posY = 0; }


		GUI.Label(new Rect(posX, posY, calcualtedSize.x, calcualtedSize.y), text);
	}

	string GetDebugInfo()
	{
		string toReturn = "Y - show/hide debug info\n";
		if (showDebugInfo)
		{
			toReturn += GetCharacterHealthInfo(player);

			foreach (var enemy in enemies)
			{
				toReturn += GetCharacterHealthInfo(enemy);
			}
		}

		return toReturn;
	}

	string GetCharacterHealthInfo(GameObject character)
	{
		return string.Format("{0} - HP: {1}/{2}, Status: {3}, Position: {4}\n", 
			character.name,
            System.Math.Round(character.GetComponent<HealthSystem>().currentHealth),
			character.GetComponent<HealthSystem>().maxHealth,
			character.GetComponent<HealthSystem>().dead ? "Dead" : "Alive",
			character.transform.position.ToString()
		);
	}
}
