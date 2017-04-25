using UnityEngine;

public class FallingMotor : BaseMotor 
{
  public override void UpdateMotor(CharacterMover mover)
  {
    motorState = MotorState.falling;

    // Move down at the speed of gravity
    mover.velocity += (mover.gravity + mover.inputVector) * Time.deltaTime;
  }

  public override void HandleCollision(CharacterMover mover, ControllerColliderHit hit)
  {
    // Check if the hit was below the player character, otherwise they should keep falling
    if (hit.point.y < transform.position.y - 0.5f)
      mover.currentState = MotorState.walking;
  }
}
