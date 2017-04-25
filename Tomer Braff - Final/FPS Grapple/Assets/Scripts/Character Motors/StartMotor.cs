using UnityEngine;

public class StartMotor : BaseMotor 
{
	public override void UpdateMotor(CharacterMover mover)
	{
    Ray r = new Ray(transform.position, Vector3.down);
    RaycastHit hit;
    Physics.SphereCast(r, .5f, out hit);

    if (hit.distance > 0.5f)
      mover.currentState = MotorState.falling;
    else
      mover.currentState = MotorState.walking;
	}
}
