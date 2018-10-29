using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public GameObject player;
    private Actions actionsComponent;
	private PlayerController playerController;
    private bool isRunningToPlayer;
	private bool isDead = false;

    void Start ()
    {    
        actionsComponent = GetComponent<Actions>();
        playerController = GetComponent<PlayerController>();
        isRunningToPlayer = false;
    }

    void Update()
	{
		if (!isDead)
		{
			int distanceFromPlayer = (int)Vector3.Distance(player.transform.position, this.transform.position);
			ChangeWeapon(distanceFromPlayer);
			FacePlayer();
			MoveReactionForPlayer(distanceFromPlayer);
			
			if (GetComponent<HealthSystem>().dead)
			{
				actionsComponent.Death();
				isDead = true;
			}
		}
	}

	void ChangeWeapon(int distanceFromPlayer)
	{
		if (distanceFromPlayer > 8)
		{
			playerController.SetArsenal(playerController.arsenal[3].name); // sniper rifle
		}
		else if (distanceFromPlayer < 1)
		{
			playerController.SetArsenal(playerController.arsenal[0].name); // fists
		}
		else
		{
			playerController.SetArsenal(playerController.arsenal[1].name); // pistol
		}
	}

	void MoveReactionForPlayer(int distanceFromPlayer)
	{
		if (distanceFromPlayer > 13 && !isRunningToPlayer)
		{
			isRunningToPlayer = true;
			actionsComponent.Run();
		}
		else if (distanceFromPlayer < 10)
		{
			isRunningToPlayer  = false;
			actionsComponent.Attack();
		}
	}

	void FacePlayer()
	{
		Vector3 direction = player.transform.position - this.transform.position; // obliczenie kierunku w ktorym ma patrzyc przeciwnik
		direction.y = 0; // bez osi y

		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f); // ustawienie rotacji
	}
}