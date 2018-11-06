using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArrow : MonoBehaviour {

	public GameObject player;
	public GameObject damageArrow;
	HealthSystem playersHealthSystem;
	float historicalHealth;
	public float maxTimeShown = 0.5f;
	float currentTimeShown = 0.0f;
	Vector3 pointing;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		playersHealthSystem = player.GetComponent<HealthSystem>();
		historicalHealth = playersHealthSystem.currentHealth;
		damageArrow.SetActive(false);
	}
	
	void Update () {

		if (historicalHealth != playersHealthSystem.currentHealth) {
			if (playersHealthSystem.lastCauseOfDamage != null)
			{
				currentTimeShown = maxTimeShown;
				historicalHealth = playersHealthSystem.currentHealth;
			}
		}	

		if (currentTimeShown > 0)
		{
			damageArrow.SetActive(true);
			currentTimeShown -= Time.deltaTime;

			var enemyPos = playersHealthSystem.lastCauseOfDamage.transform.position;
			enemyPos.y += 1f; //poprawka na wysokosc postaci przeciwnika
			Vector3 dirToEnemy = Camera.main.WorldToScreenPoint(enemyPos);
			pointing.z = Mathf.Atan2((damageArrow.transform.position.y - dirToEnemy.y), (damageArrow.transform.position.x - dirToEnemy.x)) * Mathf.Rad2Deg + 90;
			damageArrow.transform.rotation = Quaternion.Euler(pointing);
		}
		else damageArrow.SetActive(false);
	}
}
