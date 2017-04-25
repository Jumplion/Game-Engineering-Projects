using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Up"))
			transform.position += Vector3.up;
		if(Input.GetButtonDown("Down"))
			transform.position += Vector3.down;
		if(Input.GetButtonDown("Left"))
			transform.position += Vector3.left;
		if(Input.GetButtonDown("Right"))
			transform.position += Vector3.right;
	}
}
