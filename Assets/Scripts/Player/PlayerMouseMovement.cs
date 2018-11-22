using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseMovement : MonoBehaviour 
{
    public float sensitivityX = 10F, sensitivityY = 10F, minimumY = -60F, maximumY = 60F, rotationX = 0F, rotationY = 0F;
    Quaternion originalRotation, xQuaternion, yQuaternion;
	void Update () {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
        xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
        yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }
    void Start () {   
        Cursor.visible = false;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        originalRotation = transform.localRotation; // zachowanie poczatkowej rotacji
        rigidbody.freezeRotation = true; // fizyka nie wplywa na rotacje obiektu
    }
}