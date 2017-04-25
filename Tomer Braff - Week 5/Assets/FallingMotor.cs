using UnityEngine;

public class FallingMotor : BaseMotor 
{
  public float playerSpeed = 10.0f;
  public Vector3 gravity = new Vector3(0, -9.8f, 0);

  public override void UpdateMotor(CharacterMover mover)
  {
    motorState = MotorState.falling;

    // Move down at the speed of gravity
    mover.velocity += gravity;
	}

  public override void HandleCollision(CharacterMover mover, ControllerColliderHit hit)
  {
    // Check if the hit was below the player character, otherwise they should keep falling
    if(hit.point.y < transform.position.y - 0.55f)
      mover.currentState = MotorState.walking;
  }
}
