using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall : MonoBehaviour 
{
	public Vector3 gravity = new Vector3(0,-10,0);
	public float bounceForce = 10.0f;
	public float collisionBuffer = 0.01f;

	private Vector3 direction;
	private Vector3 speed;
	private Vector3 previousPos;



	// Update is called once per frame
	void Update ()
	{
		previousPos = transform.position;
		
		// Apply gravity
		speed += gravity * Time.deltaTime;
		transform.position += speed;

		UpdateSpherePhysics();
		//CheckCollision();

		Debug.DrawLine(previousPos, transform.position, Color.red, 10f);
	}

/*
	void UpdateTimeScale()
	{
		if(Input.GetButton("Fire1"))
			timeScale += timeScaleChangeRate * Time.deltaTime;
		if(Input.GetButton("Fire2"))
			timeScale -= timeScaleChangeRate * Time.deltaTime;
	}
*/

	void UpdateSpherePhysics(){
		Vector3 travelDir = speed.normalized;
		float travelDist = speed.magnitude * Time.deltaTime;
		Vector3 startPosition = transform.position;
		Vector3 currentPosition = startPosition;

		RaycastHit hitInfo;
		if(Physics.SphereCast(currentPosition, 10, travelDir, out hitInfo, travelDist))
		{
			currentPosition += travelDir * (hitInfo.distance - 0.001f);
			speed = Vector3.Reflect(speed, hitInfo.normal);
		}
		else
			currentPosition += travelDir * travelDist;

		transform.position = currentPosition;
	}

	void CheckCollision()
	{
		// If during the change in transform, the ball would have collided with something, move the ball to that position

		Vector3 deltaPos = transform.position - previousPos;
		// The direction the ball went between positions
		Vector3 deltaDirection = deltaPos.normalized;
		// How far the ball went between positions
	  float deltaDistance = deltaPos.magnitude;

		// Create ray from the previous position, in the direction the ball went between positions
		Ray ray = new Ray(previousPos, deltaDirection);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, deltaDistance))
		{
			transform.position = hit.point;
			Vector3 exitDirection = Vector3.Reflect(speed.normalized, hit.normal.normalized);
			speed = exitDirection * speed.magnitude;
		}
	}

}
