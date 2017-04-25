using UnityEngine;

public class Chaser : MonoBehaviour {

	public enum ChaseType
	{
		ConstantSpeed,
		Acceleration,
		AccelerateAndStop
	}

	public ChaseType chaseType = ChaseType.ConstantSpeed;
	public Transform target;
	public float chaserSpeed = 10.0f;
	[HeaderAttribute("Used in Acceleration and AccelerateAndStop")]
	public float maxSpeed = 10.0f;

	private Vector3 speed = Vector3.zero;

	[Header("Used in Accelerate And Stop")]
	// How fast/hard should the chaser come to a stop?
	public float decelerationAmount = 1f;
	// What angle should the chaser begin to decelerate?
	[Range(0, 180)]public float decelerateAngle = 90.0f;

	private Vector3 originalDirection = Vector3.zero;
	private bool decelerate = false;

	// Update is called once per frame
	void Update () 
	{
		if(target == null)
			return;

		Vector3 dir = (target.position - transform.position).normalized;

		switch(chaseType)
		{
			case ChaseType.ConstantSpeed: ConstantSpeed(dir);	break;
			case ChaseType.Acceleration: Acceleration(dir);		break;
			case ChaseType.AccelerateAndStop: AccelerateAndStop(dir);	break;
		}

		Debug.DrawRay(transform.position, speed * 10, Color.red);
		Debug.DrawRay(transform.position, target.position - transform.position, Color.cyan);
	}

	// Constantly move the chaser towards the target destination at a constant speed
	void ConstantSpeed(Vector3 direction)
	{
		speed = direction * chaserSpeed * Time.deltaTime;
		Vector3 newPosition = transform.position + speed;
		transform.position = newPosition;
	}

	// Accelerate the chaser towards the target destination, meaning it'll slow down as it adjusts its direction
	// Also caps it's max speed
	void Acceleration(Vector3 direction)
	{
		speed += direction * chaserSpeed * Time.deltaTime;

		if(speed.magnitude > maxSpeed)
			speed = speed.normalized * maxSpeed;

		transform.position += speed;
	}

	// Accelerate the chaser towards the target destination, and if the chaser overshoots and 
	void AccelerateAndStop(Vector3 direction)
	{	
		if(decelerate)
		{
			print("decelerating");
			speed = Vector3.Lerp(speed, Vector3.zero, decelerationAmount * Time.deltaTime);

			if(speed.magnitude <= 0.01f)
				decelerate = false;
		}
		else
		{
			speed += direction * chaserSpeed * Time.deltaTime;

			float angle = Vector3.Angle(direction, speed);
		
			//if(Mathf.Abs(angle) > decelerateAngle)
			// Dot products, if it equals/< 0, then it is greater than 90 degrees
			if(Vector3.Dot(direction, speed.normalized) < 0)
			{
				originalDirection = direction;
				decelerate = true;
			}

			if(speed.magnitude > maxSpeed)
				speed = speed.normalized * maxSpeed;
		}

		transform.position += speed;
	}
}

