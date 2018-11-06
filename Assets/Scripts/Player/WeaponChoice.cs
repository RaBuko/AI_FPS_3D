using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponChoice : MonoBehaviour {

	List<GameObject> arsenal;

	GameObject chosenWeapon;

	// Use this for initialization
	void Start () {
		arsenal = GameObject.FindGameObjectsWithTag("PlayersWeapon").ToList();
		arsenal.ForEach(x => x.SetActive(false));
		chosenWeapon = arsenal.Find(x => x.name == "Pistol");
		chosenWeapon.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			chosenWeapon.SetActive(false);
			chosenWeapon = arsenal.Find(x => x.name == "Pistol");
			chosenWeapon.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			chosenWeapon.SetActive(false);
			chosenWeapon = arsenal.Find(x => x.name == "M4");
			chosenWeapon.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			chosenWeapon.SetActive(false);
			chosenWeapon = arsenal.Find(x => x.name == "Shotgun");
			chosenWeapon.SetActive(true);
		}
	}
}
