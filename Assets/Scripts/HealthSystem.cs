using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
	public bool canDie = true;
	public bool dead = false;
	public float startingHealth = 100.0f;
	public float currentHealth;	
	public float maxHealth = 100.0f;


	public void ChangeHealth(float amount)
	{
		if (!dead)
		{
			currentHealth += amount;

			if (currentHealth <= 0 && canDie)
			{
				dead = true;
			}

			else if (currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
		}
		else
		{
			currentHealth = 0;
		}
	}

	void Start()
	{
		currentHealth = startingHealth;
	}
}
