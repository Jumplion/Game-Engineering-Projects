using UnityEngine;

public class WalkingMotor : BaseMotor
{
  public float playerSpeed = 5.0f;

  public override void UpdateMotor(CharacterMover mover)
	{
    if(motorState != MotorState.falling)
    {
      motorState = MotorState.walking;

      mover.velocity = Vector3.zero;

      Vector3 input = mover.inputVector * playerSpeed;

      // If jumping, switch it to falling
      if (Input.GetKeyDown(KeyCode.Space))
      {
        input += new Vector3(0, Mathf.Sqrt(2 * mover.jumpHeight * -mover.gravity.y), 0);
        mover.currentState = MotorState.falling;
      }

      mover.velocity = input;
    }
  }
}
