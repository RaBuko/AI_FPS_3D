using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseMovement : MonoBehaviour 
{
	public float sensitivityX = 10F, sensitivityY = 10F; // czułosc myszy
	public float minimumY = -60F, maximumY = 60F; // min i max kat osi Y
    public float rotationX = 0F, rotationY = 0F;
    private List<float> rotArrayX = new List<float>();
    float sumRotationX;
    private List<float> rotArrayY = new List<float>();
    float sumRotationY;
    public float frameCounter = 5;
    Quaternion originalRotation;
	
	void Update ()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
       
        rotArrayY.Add(rotationY);
        rotArrayX.Add(rotationX);
       
        if (rotArrayY.Count >= frameCounter) {
            rotArrayY.RemoveAt(0);
        }
        if (rotArrayX.Count >= frameCounter) {
            rotArrayX.RemoveAt(0);
        }

       	sumRotationX = 0F;
	   	sumRotationY = 0F;
       
        for(int j = 0; j < rotArrayY.Count; j++) {
            sumRotationY += rotArrayY[j];
        }
        for(int i = 0; i < rotArrayX.Count; i++) {
            sumRotationX += rotArrayX[i];
        }
       
        sumRotationY /= rotArrayY.Count;
        sumRotationX /= rotArrayX.Count;
       
        sumRotationY = Mathf.Clamp (sumRotationY, minimumY, maximumY);
       
    	Quaternion xQuaternion = Quaternion.AngleAxis (sumRotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis (sumRotationY, Vector3.left);
       
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }
    void Start ()
    {   
        Cursor.visible = false;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        originalRotation = transform.localRotation; // zachowanie poczatkowej rotacji
        rigidbody.freezeRotation = true; // fizyka nie wplywa na rotacje obiektu
    }
}