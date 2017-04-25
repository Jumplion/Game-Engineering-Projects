using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunMotor : BaseMotor
{
  public LayerMask wallRunLayer;
  public LayerMask groundLayer;

  public float speed = 5.0f;
  public float wallBuffer = 0.01f;
  public float jumpForce = 1.0f;

  [System.NonSerialized] public RaycastHit wallHit;
  [System.NonSerialized] public Vector3 wallRunForward;

  public override void UpdateMotor(CharacterMover mover)
  {
    mover.currentState = MotorState.wallRunning;

    // Move the player forward in that parallel direction
    // For now, no time limit on how long the player can run on a wall
    mover.velocity = wallRunForward * mover.velocity.magnitude;

    if (Input.GetKeyDown(KeyCode.Space))
    {
      mover.velocity += (transform.forward + wallHit.normal + Vector3.up).normalized * jumpForce;
      mover.currentState = MotorState.falling;
    }

    if(mover.velocity == Vector3.zero)
      mover.currentState = MotorState.falling;

  }

  public override void HandleCollision(CharacterMover mover, ControllerColliderHit hit)
  {
    // Falling so that the falling motor will place them back to a proper distance on the ground just in case it's at an awkward angle
    if(hit.gameObject.layer == groundLayer)
      mover.currentState = MotorState.falling;
  }
}
