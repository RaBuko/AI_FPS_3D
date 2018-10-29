using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardMovement : MonoBehaviour 
{

	public float horizontalMovementSpeed = 150.0F;
	public float verticalMovementSpeed = 150.0F;

	void Update () 
	{
		var moveVertical = Input.GetAxis("Vertical") * Time.deltaTime * verticalMovementSpeed;
		var moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalMovementSpeed;

		transform.Translate(moveHorizontal, 0, moveVertical);
	}
}
