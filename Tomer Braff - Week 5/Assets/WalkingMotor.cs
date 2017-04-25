using UnityEngine;

public class WalkingMotor : BaseMotor
{
  public float playerSpeed = 10.0f;
  public float jumpForce = 10.0f;

  public override void UpdateMotor(CharacterMover mover)
	{
    if(motorState != MotorState.falling)
    {
      motorState = MotorState.walking;

      Vector3 moveDirection = Vector3.zero;
      mover.velocity = Vector3.zero;

      if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        moveDirection += Vector3.forward * playerSpeed;
      if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        moveDirection += Vector3.back * playerSpeed;
      if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        moveDirection += Vector3.right * playerSpeed;
      if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        moveDirection += Vector3.left * playerSpeed;
    
      // If jumping, switch it to falling
      if (Input.GetKeyDown(KeyCode.Space))
      {
        moveDirection += Vector3.up * jumpForce;
        mover.currentState = MotorState.falling;
      }

      mover.velocity = moveDirection;
    }
  }
}
