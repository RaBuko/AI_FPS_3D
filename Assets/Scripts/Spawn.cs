using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawn : MonoBehaviour {
	public GameObject gameObjectToSpawn;
	public float timeBetweenSpawns = 10.0f;
	public float spawnTime = 0.0f;
	public Transform lastSpawner;
	public float timeBetweenSpawnsModifier = 0.5f;
	
	void Start() {
		spawnTime = timeBetweenSpawns;
		lastSpawner = transform.Find("Spawn");
	}
	
	void Update() {
		spawnTime -= Time.deltaTime;
		if (spawnTime <= 0) {
			if (timeBetweenSpawns > 1) {
				timeBetweenSpawns -= timeBetweenSpawnsModifier;
			}
			spawnTime = timeBetweenSpawns;
			lastSpawner = ChooseSpawner();
			Instantiate(gameObjectToSpawn, lastSpawner.transform.position, Quaternion.identity);
		}
	}

	Transform ChooseSpawner() {
		var children = new List<Transform>();
		foreach (var child in transform) {
			children.Add((Transform)child);
		}
		return children.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
	}
}
