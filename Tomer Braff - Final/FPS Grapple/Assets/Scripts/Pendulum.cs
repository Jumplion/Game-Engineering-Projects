using UnityEngine;
using System.Collections;

// Author: Eric Eastwood (ericeastwood.com)
//
// Description:
//      Written for this gd.se question: http://gamedev.stackexchange.com/a/75748/16587
//      Simulates/Emulates pendulum motion in code
//      Works in any 3D direction and with any force/direciton of gravity
//
// Demonstration: https://i.imgur.com/vOQgFMe.gif
//
// Usage: https://i.imgur.com/BM52dbT.png

public class Pendulum : MonoBehaviour
{
  // The point where the game object swings around
  public GameObject Pivot;
  public float ropeLength = 2f;
  public float mass = 1f;
  public Vector3 gravity = new Vector3(0, -10, 0);

  // Keep track of the current velocity
  [SerializeField] private Vector3 currentVelocity;

  // We use these to smooth between values in certain framerate situations in the `Update()` loop
  Vector3 currentStatePosition;
  Vector3 previousStatePosition;

  // Use this for initialization
  void Start()
  {
    // Get the initial rope length from how far away the bob is now
    //ropeLength = Vector3.Distance(Pivot.transform.position, transform.position);
    //PendulumInit();
    currentStatePosition = transform.position;
  }

  float t = 0f;
  float dt = 0.01f;
  float currentTime = 0f;
  float accumulator = 0f;

  void Update()
  {
    // Fixed deltaTime rendering at any speed with smoothing
    // Technique: http://gafferongames.com/game-physics/fix-your-timestep/

    float frameTime = Time.time - currentTime;
    currentTime = Time.time;

    accumulator += frameTime;

    while (accumulator >= dt)
    {
      previousStatePosition = currentStatePosition;
      currentStatePosition = PendulumUpdate(currentStatePosition, dt);
      //integrate(state, t, dt);
      accumulator -= dt;
      t += dt;
    }

    float alpha = accumulator / dt;

    Vector3 newPosition = currentStatePosition * alpha + previousStatePosition * (1f - alpha);

    transform.position = newPosition; //currentStatePosition;

    //Bob.transform.position = PendulumUpdate(Bob.transform.position, Time.deltaTime);
  }

  Vector3 PendulumUpdate(Vector3 currentStatePosition, float deltaTime)
  {
    // You could define these in the `PendulumUpdate()` loop 
    // But we want them in the class scope so we can draw gizmos `OnDrawGizmos()`
    Vector3 gravityDirection = gravity.normalized;
    Vector3 pivot_p = Pivot.transform.position;
    Vector3 bob_p = currentStatePosition;

    // Add gravity free fall
    float gravityForce = mass * gravity.magnitude;

    currentVelocity += gravityDirection * gravityForce * deltaTime;

    Vector3 auxiliaryMovementDelta = currentVelocity * deltaTime;
    float distanceAfterGravity = Vector3.Distance(pivot_p, bob_p + auxiliaryMovementDelta);

    // If at the end of the rope
    if (distanceAfterGravity > ropeLength || Mathf.Approximately(distanceAfterGravity, ropeLength))
    {
      Vector3 tensionDirection = (pivot_p - bob_p).normalized;

      Vector3 pendulumSideDirection = (Quaternion.Euler(0f, 90f, 0f) * tensionDirection);
      pendulumSideDirection.Scale(new Vector3(1f, 0f, 1f));
      pendulumSideDirection.Normalize();

      Vector3 tangentDirection = (-1f * Vector3.Cross(tensionDirection, pendulumSideDirection)).normalized;

      float inclinationAngle = Vector3.Angle(bob_p - pivot_p, gravityDirection);

      float tensionForce = mass * gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad * inclinationAngle);
      float centripetalForce = ((mass * Mathf.Pow(currentVelocity.magnitude, 2)) / ropeLength);
      tensionForce += centripetalForce;

      currentVelocity += tensionDirection * tensionForce * deltaTime;
    }

    // Get the movement delta
    Vector3 movementDelta = currentVelocity * deltaTime;

    //return currentStatePosition + movementDelta;

    // This is probably what forces the ball on the edge
    float distance = Vector3.Distance(pivot_p, currentStatePosition + movementDelta);

    return GetPointOnLine(pivot_p, currentStatePosition + movementDelta, distance <= ropeLength ? distance : ropeLength);
  }

  Vector3 GetPointOnLine(Vector3 start, Vector3 end, float distanceFromStart)
  {
    return start + (distanceFromStart * Vector3.Normalize(end - start));
  }

  #region Reset Functions, not needed for now
  /*
  // Use this to reset forces and go back to the starting position
  [ContextMenu("Reset Pendulum Position")]
  void ResetPendulumPosition()
  {
    if (startingPositionSet)
      ResetBob(startingPosition);
    else
      PendulumInit();
  }

  // Use this to reset any built up forces
  [ContextMenu("Reset Pendulum Forces")]
  void ResetPendulumForces()
  {
    currentVelocity = Vector3.zero;

    // Set the transition state
    currentStatePosition = transform.position;
  }

  void PendulumInit()
  {
    // Get the initial rope length from how far away the bob is now
    ropeLength = Vector3.Distance(Pivot.transform.position, transform.position);

    currentStatePosition = transform.position;
    //ResetPendulumForces();
  }

  void ResetBob(Vector3 resetBobPosition)
  {
    // Put the bob back in the place we first saw it at in `Start()`
    transform.position = resetBobPosition;

    // Set the transition state
    currentStatePosition = resetBobPosition;
  }
  */
  #endregion

  /*
    void OnDrawGizmos()
    {
      // purple
      Gizmos.color = new Color(.5f, 0f, .5f);
      Gizmos.DrawWireSphere(Pivot.transform.position, ropeLength);

      Gizmos.DrawWireCube(bobStartingPosition, new Vector3(.5f, .5f, .5f));


      // Blue: Auxilary
      Gizmos.color = new Color(.3f, .3f, 1f); // blue
      Vector3 auxVel = .3f * currentVelocity;
      Gizmos.DrawRay(Bob.transform.position, auxVel);
      Gizmos.DrawSphere(Bob.transform.position + auxVel, .2f);

      // Yellow: Gravity
      Gizmos.color = new Color(1f, 1f, .2f);
      Vector3 gravity = .3f * gravityForce * gravityDirection;
      Gizmos.DrawRay(Bob.transform.position, gravity);
      Gizmos.DrawSphere(Bob.transform.position + gravity, .2f);

      // Orange: Tension
      Gizmos.color = new Color(1f, .5f, .2f); // Orange
      Vector3 tension = .3f * tensionForce * tensionDirection;
      Gizmos.DrawRay(Bob.transform.position, tension);
      Gizmos.DrawSphere(Bob.transform.position + tension, .2f);

      // Red: Resultant
      Gizmos.color = new Color(1f, .3f, .3f); // red
      Vector3 resultant = gravity + tension;
      Gizmos.DrawRay(Bob.transform.position, resultant);
      Gizmos.DrawSphere(Bob.transform.position + resultant, .2f);


      ////////
      // Green: Pendulum side direction
      Gizmos.color = new Color(.3f, 1f, .3f);
      Gizmos.DrawRay(Bob.transform.position, 3f*pendulumSideDirection);
      Gizmos.DrawSphere(Bob.transform.position + 3f*pendulumSideDirection, .2f);
      ////////

      ////////
      // Cyan: tangent direction
      Gizmos.color = new Color(.2f, 1f, 1f); // cyan
      Gizmos.DrawRay(Bob.transform.position, 3f*tangentDirection);
      Gizmos.DrawSphere(Bob.transform.position + 3f*tangentDirection, .2f);
      ////////
    }
  */
}