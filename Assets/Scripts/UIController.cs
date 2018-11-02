using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour {

	private Version programVersion = new Version("1.0.0");
	private GameObject player;
	private List<GameObject> enemies;
	private GameObject spawner;
	private GUIStyle style;
	private float nativeSize = 800.0f;

	bool showDebugInfo = false;

	void Start () {
		style = new GUIStyle();
		style.fontSize = (int)(10.0f * ((float)Screen.width / nativeSize));
		player = GameObject.FindGameObjectWithTag("Player");
		spawner = GameObject.FindGameObjectWithTag("Spawner");
	}

	void Update()
	{
		enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
		if (Input.GetKeyDown(KeyCode.Y))
		{
			showDebugInfo = showDebugInfo ? false : true;
		}
	}
	
	void OnGUI()
	{
		ShowLabel(Screen.width - 10, Screen.height - 10, GetPlayerInfo());
		ShowLabel(Screen.width - 10, 10, string.Format("Copyright 2018 Rafał Bukowski\nv.{0}", programVersion, true));
		ShowLabel(10, 10, GetDebugInfo());
	}

	void ShowLabel(float posX, float posY, string text, bool alignToRight = false)
	{
		var calcualtedSize = style.CalcSize(new GUIContent(text));

		if (alignToRight) { posX -= calcualtedSize.x; }

		if (posX + calcualtedSize.x > Screen.width) { posX = Screen.width - calcualtedSize.x; }
		if (posY + calcualtedSize.y > Screen.height) { posY = Screen.height - calcualtedSize.y; }
		if (posX < 0) { posX = 0; }
		if (posY < 0) { posY = 0; }


		GUI.Label(new Rect(posX, posY, calcualtedSize.x, calcualtedSize.y), text, style);
	}

	string GetPlayerInfo()
	{
		return string.Format("Health: {0}\nFrags: {1}\nAlive enemies: {2}", 
			System.Math.Round(player.GetComponent<HealthSystem>().currentHealth),
			enemies.Count() - GetNumberOfAliveEnemies(),
			GetNumberOfAliveEnemies()	
		);
	}

	string GetDebugInfo()
	{
		string toReturn = "Y - show/hide debug info\n";
		if (showDebugInfo)
		{
			toReturn += string.Format("Number of enemies (alive/all) : {0}/{1}", enemies.Where(x => !x.GetComponent<Enemy>().isDead).Count(), enemies.Count());
			toReturn += GetSpawnerInfo() + Environment.NewLine;
			toReturn += GetCharacterHealthInfo(player) + Environment.NewLine;

			foreach (var enemy in enemies)
			{
				toReturn += GetCharacterHealthInfo(enemy);
				toReturn += GetEnemyAdditionalInfo(enemy);
				toReturn += Environment.NewLine;
			}
		}

		return toReturn;
	}

	string GetCharacterHealthInfo(GameObject character)
	{
		return string.Format("{0} - HP: {1}/{2}, IsDead: {3}, Position: {4}", 
			character.name,
            System.Math.Round(character.GetComponent<HealthSystem>().currentHealth),
			character.GetComponent<HealthSystem>().maxHealth,
			character.GetComponent<HealthSystem>().dead,
			character.transform.position.ToString()
		);
	}

	string GetEnemyAdditionalInfo(GameObject enemy)
	{
		var enemyData = enemy.GetComponent<Enemy>();
		return string.Format(", PlayerDetected: {0}, IsRunning: {1}, TimeLastAttack: {2}",
			enemyData.playerDetected,
			enemyData.isRunning,
			System.Math.Round(enemyData.timeToAttack, 2)
		);
	}

	string GetSpawnerInfo()
	{
		return string.Format("Spawner - LastSpawnPosition: {0}, TimeToNextSpawn: {1}",
			spawner.GetComponent<Spawn>().lastSpawner.transform.position,
			spawner.GetComponent<Spawn>().spawnTime
		);
	}

	int GetNumberOfAliveEnemies()
	{	
		int count = 0;
		foreach (var enemy in enemies) if (!enemy.GetComponent<Enemy>().isDead) count++;
		return count;
	}
}
