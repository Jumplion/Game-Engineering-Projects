using UnityEngine;

public class Ball : MonoBehaviour 
{
  public GameObject swingPoint;
  public float swingRadius = 5.0f;

	public Vector3 gravity = new Vector3(0,-10,0);

	public Color pathColor = Color.red;
	public float pathDuration = 1.0f;

	private Vector3 direction;
	private Vector3 velocity;
	private Vector3 previousPos;

  private void Update()
  {
    velocity = velocity + gravity * Time.deltaTime;
    Vector3 testPosition = transform.position + velocity * Time.deltaTime;
    
    RaycastHit intersection;
    if (Physics.Raycast(transform.position, testPosition, out intersection))
    {
      // we went through a wall, let's pull the character back,
      // using the normal of the wall
      // with a 1-unit space for breathing room
      testPosition = intersection.point + intersection.normal;
    }

    
    if (swingPoint != null)
    {
      Vector3 ballUp = testPosition - swingPoint.transform.position;
      if (ballUp.magnitude > swingRadius )
      {
        // we're past the end of our rope
        // pull the avatar back in.
        testPosition = ballUp.normalized * swingRadius;
      }
    }
    

    velocity = (testPosition - transform.position) / Time.deltaTime;
    transform.position = testPosition;

    Debug.DrawRay(transform.position, velocity, Color.red);
  }

  /*
	// Update is called once per frame
	void Update ()
	{
		previousPos = transform.position;
		
		// Apply gravity
		velocity += gravity * Time.deltaTime;

    if (swingPoint != null)
    {
      Vector3 myUp = (swingPoint.transform.position - transform.position);
      transform.rotation = Quaternion.LookRotation(velocity, myUp);

      if (myUp.magnitude > swingRadius)
      {
        //currentPos = (currentPos - swingPoint.transform.position).normalized * swingRadius;
      }

      Debug.DrawRay(transform.position, myUp, Color.cyan);
      //Debug.DrawLine(transform.position, swingPoint.transform.position, Color.green);
    }

    CheckCollision();

		//transform.position += velocity;
		
		CheckCollision();

		Debug.DrawLine(previousPos, transform.position, pathColor);
	}
  */

	void CheckCollision()
	{
		// If during the change in transform, the ball would have collided with something, move the ball to that position

		Vector3 startPos = transform.position;
		Vector3 currentPos = startPos;
		Vector3 moveDelta = velocity * Time.deltaTime;

		RaycastHit hit;
		if(Physics.SphereCast(startPos, 0.5f, moveDelta.normalized, out hit, moveDelta.magnitude))
		{
			currentPos += moveDelta.normalized * hit.distance;
			velocity = Vector3.Reflect(velocity, hit.normal);	
			hit.transform.SendMessage("OnBounce", this, SendMessageOptions.DontRequireReceiver);
		}
		else
			currentPos += moveDelta;



		transform.position = currentPos;
	}

	public void ReverseGravity()
	{
		gravity = -gravity;
	}
}