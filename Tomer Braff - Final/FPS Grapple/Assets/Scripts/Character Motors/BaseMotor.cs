using UnityEngine;

// Abstract forces this class to be unable to instantiate this specific version of it
public abstract class BaseMotor : MonoBehaviour 
{
	public MotorState motorState;
	public virtual void UpdateMotor(CharacterMover mover){}
  public virtual void HandleCollision(CharacterMover mover, ControllerColliderHit hit) { }
}