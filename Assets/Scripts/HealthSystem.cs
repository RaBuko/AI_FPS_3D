using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour {
	public bool dead = false;
	public float currentHealth;	
	public float maxHealth = 100.0f;
	public GameObject lastCauseOfDamage;
	public void ChangeHealth(float amount) {
		if (!dead) {
			currentHealth += amount;

			if (currentHealth <= 0) {
				dead = true;
			}
		}
		else {
			currentHealth = 0;
		}
	}

	void Start() {
		currentHealth = maxHealth;
	}
}
