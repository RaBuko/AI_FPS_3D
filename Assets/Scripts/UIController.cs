using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	private Version programVersion = new Version("1.2.4");
	private GameObject player;
	private List<GameObject> enemies;
	private GameObject spawner;
	private GUIStyle style;
	private float nativeSize = 800.0f;
	public Vector2 HealthBarSize;

	bool showDebugInfo = false;

	void Start () {
		style = new GUIStyle();
		style.fontSize = (int)(10.0f * ((float)Screen.width / nativeSize));
		player = GameObject.FindGameObjectWithTag("Player");
		spawner = GameObject.FindGameObjectWithTag("Spawner");
		HealthBarSize = new Vector2(400, 50);
	}

	void Update()
	{
		if (player.GetComponent<HealthSystem>().currentHealth < 0)
		{
			PlayerPrefs.SetInt("LastKillRatio", enemies.Count() - GetNumberOfAliveEnemies());
			SceneManager.LoadScene(1);
		}
		enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
		if (Input.GetKeyDown(KeyCode.Y))
		{
			showDebugInfo = showDebugInfo ? false : true;
		}
		else if (Input.GetKeyDown(KeyCode.U))
		{
       		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void OnGUI()
	{
		ShowLabel(Screen.width - 15, Screen.height - 15, GetPlayerInfo(), true);
		ShowLabel(Screen.width - 15, 15, string.Format("Copyright 2018 Rafał Bukowski\nv.{0}", programVersion, true));
		ShowLabel(15, 15, GetControlsInfo() + (showDebugInfo ? GetDebugInfo() : String.Empty));

		ShowLabel(Screen.width / 2 - style.CalcSize(new GUIContent("Health")).x / 2, Screen.height - HealthBarSize.y - 20, "Health");
		DrawRect(new Rect(Screen.width / 2 - HealthBarSize.x / 2, Screen.height - HealthBarSize.y / 2 - 10, HealthBarSize.x, HealthBarSize.y), Color.black);
		DrawRect(new Rect(Screen.width / 2 - HealthBarSize.x / 2, Screen.height - HealthBarSize.y / 2 - 10, (HealthBarSize.x / player.GetComponent<HealthSystem>().maxHealth) * player.GetComponent<HealthSystem>().currentHealth, HealthBarSize.y), Color.green);
	}

	void DrawRect(Rect position, Color color)
	{
		Texture2D rectTexture = new Texture2D(1, 1);
		rectTexture.SetPixel(0,0,color);
		rectTexture.Apply();
		GUI.skin.box.normal.background = rectTexture;
		GUI.Box(position, GUIContent.none);
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
		return string.Format("Frags: {0}\nAlive enemies: {1}",
			enemies.Count() - GetNumberOfAliveEnemies(),
			GetNumberOfAliveEnemies()	
		);
	}

	string GetDebugInfo()
	{
		string toReturn = string.Empty;
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

	string GetControlsInfo()
	{
		return @"Esc - quit the game
U - reset the game
Y - show/hide debug info
W,A,S,D or arrows - moving the player
1,2,3 - choice of weapons
R - reload weapon
";
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
		if (enemyData.isDead) return String.Empty;
		else return string.Format(", PlayerDetected: {0}, IsRunning: {1}, TimeLastAttack: {2}",
			enemyData.playerDetected,
			enemyData.isRunning,
			System.Math.Round(enemyData.timeToAttack, 2)
		);
	}

	string GetSpawnerInfo()
	{
		return string.Format("Spawner - LastSpawnPosition: {0}, TimeToNextSpawn: {1}",
			spawner.GetComponent<Spawn>().lastSpawner.transform.position,
			Mathf.Round(spawner.GetComponent<Spawn>().spawnTime)
		);
	}

	int GetNumberOfAliveEnemies()
	{	
		int count = 0;
		foreach (var enemy in enemies) if (!enemy.GetComponent<Enemy>().isDead) count++;
		return count;
	}
}
