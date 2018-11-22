using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardMovement : MonoBehaviour  {
	public float horizontalMovementSpeed = 150.0F, verticalMovementSpeed = 150.0F;
	private System.Single moveVertical, moveHorizontal;
	void Update () {
		moveVertical = Input.GetAxis("Vertical") * Time.deltaTime * verticalMovementSpeed;
		moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalMovementSpeed;

		transform.Translate(moveHorizontal, 0, moveVertical);
	}
}
