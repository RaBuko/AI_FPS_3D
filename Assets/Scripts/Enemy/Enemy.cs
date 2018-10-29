using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour 
{
	public GameObject player;
	public float AmountOfDamageMin = 3.0f;
	public float AmountOfDamageMax = 5.0f;



    private Actions actionsComponent;
	private PlayerController playerController;
    private bool isRunningToPlayer;
	private bool isDead = false;
	private bool playerDetected = true;
	private float timeBetweenAttacks = 1.7f;
	private float timeFromLastAttack = 0.0f;
	private int distanceRunCloser = 13;
	private int distanceStopRunningCloser = 10;
	private bool playerIsVisible = false;
	private float rayRange = 100f;



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
			CheckVisibilityOfPlayer();
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
		timeFromLastAttack += Time.deltaTime;
		if (distanceFromPlayer > distanceRunCloser && !isRunningToPlayer || !playerDetected)
		{
			isRunningToPlayer = true;
			actionsComponent.Run();
		}
		else if (distanceFromPlayer < distanceStopRunningCloser && playerDetected)
		{
			isRunningToPlayer = false;
			if (timeFromLastAttack > timeBetweenAttacks)
			{
				timeFromLastAttack = 0.0f;
				actionsComponent.Attack();
				DealDamageToPlayer();
			}
		}
	}

	void FacePlayer()
	{
		Vector3 direction = player.transform.position - this.transform.position; // obliczenie kierunku w ktorym ma patrzyc przeciwnik
		direction.y = 0; // bez osi y

		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f); // ustawienie rotacji
	}

	void DealDamageToPlayer()
	{
		float dmg = Random.Range(AmountOfDamageMin,  AmountOfDamageMax);
		player.GetComponent<HealthSystem>().ChangeHealth(-dmg);
	}

	void CheckVisibilityOfPlayer()
	{
		var origin = transform.position;
		origin.y += 0.5f;
		Ray ray = new Ray(origin, transform.forward);
		Debug.DrawRay(transform.position, transform.forward, Color.green);
		var hits = Physics.RaycastAll(ray).OrderBy(x => x.distance);

		foreach (var hit in hits)
		{
			if (hit.collider.gameObject.CompareTag("Obstacle"))
			{
				playerDetected = false;
				break;
			}
			else if (hit.collider.gameObject.CompareTag("Player"))
			{
				playerDetected = true;
				break;
			}
		}

		Debug.Log(playerDetected);
	}
}