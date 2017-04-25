using UnityEngine;

public enum MotorState
{
	start,
	walking,
	falling
}

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour 
{
  [HideInInspector]
	public CharacterController charController;
  public Vector3 velocity;

	public MotorState currentState;

  [HideInInspector]
  public BaseMotor walkingMotor;
  [HideInInspector]
  public BaseMotor fallingMotor;
  [HideInInspector]
  public BaseMotor startMotor;

	private void Awake()
	{
		charController = GetComponent<CharacterController>();
		walkingMotor = GetComponent<WalkingMotor>();
		fallingMotor = GetComponent<FallingMotor>();
		startMotor = GetComponent<StartMotor>();
	}

	private void Update()
	{
    switch (currentState)
    {
      case MotorState.start:
        startMotor.UpdateMotor(this);
        break;
      case MotorState.walking:
        walkingMotor.UpdateMotor(this);
        break;
      case MotorState.falling:
        fallingMotor.UpdateMotor(this);
        break;
    }

    charController.Move(velocity * Time.deltaTime);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		switch(currentState)
		{
			case MotorState.start:
				startMotor.HandleCollision(this, hit);
				break;
			case MotorState.walking:
				walkingMotor.HandleCollision(this, hit);
				break;
			case MotorState.falling:
				fallingMotor.HandleCollision(this, hit);
				break;
		}
	}

  public bool CheckForFloor()
  {
    RaycastHit hitInfo;
    if (Physics.SphereCast(transform.position, 1.0f, Vector3.down, out hitInfo, 0.5f))
      return Vector3.Dot(Vector3.up, hitInfo.normal) > 0.7f;

    return false;
  }

  public bool CheckForFloor(out RaycastHit hit)
  {
    if(Physics.SphereCast(transform.position, 1.0f, Vector3.down, out hit, 0.5f))
      return Vector3.Dot(Vector3.up, hit.normal) > 0.7f;

    return false;
  }


  //public bool IsHitFloor(ControllerColliderHit )
}
