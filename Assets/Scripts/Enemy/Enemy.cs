using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
	private GameObject player;

    private Actions actionsComponent;
	private PlayerController playerController;
	private NavMeshAgent navMeshAgent;

	public float AmountOfDamageMin = 3.0f;
	public float AmountOfDamageMax = 5.0f;
	public float timeBetweenAttacks = 1.7f;
	public int shootRange = 13;
	public int minDistanceForSniperRifle = 7;
	public int maxDistanceForFists = 1;

    public bool isRunning;
	public bool isDead = false;
	public bool playerDetected = true;

	public float timeToAttack = 0.0f;

    void Start () {   
		player = GameObject.FindGameObjectWithTag("Player");
        actionsComponent = GetComponent<Actions>();
        playerController = GetComponent<PlayerController>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		timeToAttack = timeBetweenAttacks;
    }

    void Update() {
		if (!isDead) {
			int distanceFromPlayer = (int)Vector3.Distance(player.transform.position, this.transform.position);
			playerDetected = CheckVisibilityOfPlayer();

			if (playerDetected) {
				FacePlayer();
				ChangeWeapon(distanceFromPlayer);
			}

			ReactToPlayer(distanceFromPlayer);
			
			if (GetComponent<HealthSystem>().dead) {
				actionsComponent.Death();
				navMeshAgent.isStopped = true;
				isDead = true;
				Destroy(GetComponent<Rigidbody>(), 2);
				Destroy(GetComponent<BoxCollider>(), 2);
			}
		}
	}

	void ChangeWeapon(int distanceFromPlayer) {

		if (distanceFromPlayer > minDistanceForSniperRifle) {
			playerController.SetArsenal(playerController.arsenal[3].name); // sniper rifle
		}
		else if (distanceFromPlayer < maxDistanceForFists) {
			playerController.SetArsenal(playerController.arsenal[0].name); // fists
		}
		else {
			playerController.SetArsenal(playerController.arsenal[1].name); // pistol
		}
	}

	void ReactToPlayer(int distanceFromPlayer) {
		timeToAttack -= Time.deltaTime;
		isRunning = !navMeshAgent.isStopped;

		if (!playerDetected || (distanceFromPlayer > shootRange && navMeshAgent.isStopped)) {
			timeToAttack = timeBetweenAttacks;
			navMeshAgent.isStopped = false;
			navMeshAgent.SetDestination(player.transform.position);
			actionsComponent.Run();
		}
		else if (distanceFromPlayer < shootRange && playerDetected) {
			navMeshAgent.isStopped = true;
			if (timeToAttack <= 0) {
				timeToAttack = timeBetweenAttacks;
				FacePlayer();
				actionsComponent.Attack();
				DealDamageToPlayer();
			}
		}
	}

	void DealDamageToPlayer() {
		float dmg = Random.Range(AmountOfDamageMin,  AmountOfDamageMax);
		player.GetComponent<HealthSystem>().ChangeHealth(-dmg);
		player.GetComponent<HealthSystem>().lastCauseOfDamage = gameObject;
	}

	bool CheckVisibilityOfPlayer() {
		
		var hits = Physics.RaycastAll(transform.position, player.transform.position - transform.position, 1000).OrderBy(x => x.distance);
		foreach (var hit in hits) {
			var tag = hit.collider.gameObject.tag;
			if (tag.Equals("Obstacle")) {return false; }
			if (tag.Equals("Player")) {return true; }
		}
		return false;
	}

	void FacePlayer() {
		Vector3 direction = player.transform.position - this.transform.position;
		direction.y = 0;
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.5f);
	}
}