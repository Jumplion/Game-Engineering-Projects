﻿using UnityEngine;
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

  // How heavy the object that swings around is
  public float mass = 1f;

  // Length of the rope to swing on
  float ropeLength = 2f;

  Vector3 startingPosition;
  bool startingPositionSet = false;

  // Keep track of the current velocity
  public Vector3 currentVelocity;

  // We use these to smooth between values in certain framerate situations in the `Update()` loop
  Vector3 currentStatePosition;
  Vector3 previousStatePosition;


  // Use this for initialization
  void Start()
  {
    // Set the starting position for later use in the context menu reset methods
    this.startingPosition = transform.position;
    this.startingPositionSet = true;

    // Get the initial rope length from how far away the bob is now
    this.ropeLength = Vector3.Distance(Pivot.transform.position, transform.position);

    this.currentStatePosition = transform.position;

    //this.PendulumInit();
  }


  float t = 0f;
  float dt = 0.01f;
  float currentTime = 0f;
  float accumulator = 0f;

  void Update()
  {
    /* */
    // Fixed deltaTime rendering at any speed with smoothing
    // Technique: http://gafferongames.com/game-physics/fix-your-timestep/
    float frameTime = Time.time - currentTime;
    this.currentTime = Time.time;

    this.accumulator += frameTime;

    while (this.accumulator >= this.dt)
    {
      this.previousStatePosition = this.currentStatePosition;
      this.currentStatePosition = this.PendulumUpdate(this.currentStatePosition, this.dt);
      //integrate(state, this.t, this.dt);
      accumulator -= this.dt;
      this.t += this.dt;
    }

    float alpha = this.accumulator / this.dt;

    Vector3 newPosition = this.currentStatePosition * alpha + this.previousStatePosition * (1f - alpha);

    transform.position = newPosition; //this.currentStatePosition;
                                               /* */

    //this.Bob.transform.position = this.PendulumUpdate(this.Bob.transform.position, Time.deltaTime);
  }

  Vector3 PendulumUpdate(Vector3 currentStatePosition, float deltaTime)
  {
    // You could define these in the `PendulumUpdate()` loop 
    // But we want them in the class scope so we can draw gizmos `OnDrawGizmos()`
    Vector3 gravityDirection;
    Vector3 tensionDirection;

    Vector3 tangentDirection;
    Vector3 pendulumSideDirection;

    float tensionForce = 0f;
    float gravityForce = 0f;

    // Add gravity free fall
    gravityForce = this.mass * Physics.gravity.magnitude;
    gravityDirection = Physics.gravity.normalized;
    currentVelocity += gravityDirection * gravityForce * deltaTime;

    Vector3 pivot_p = this.Pivot.transform.position;
    Vector3 bob_p = this.currentStatePosition;


    Vector3 auxiliaryMovementDelta = this.currentVelocity * deltaTime;
    float distanceAfterGravity = Vector3.Distance(pivot_p, bob_p + auxiliaryMovementDelta);

    // If at the end of the rope
    if (distanceAfterGravity > this.ropeLength || Mathf.Approximately(distanceAfterGravity, this.ropeLength))
    {

      tensionDirection = (pivot_p - bob_p).normalized;

      pendulumSideDirection = (Quaternion.Euler(0f, 90f, 0f) * tensionDirection);
      pendulumSideDirection.Scale(new Vector3(1f, 0f, 1f));
      pendulumSideDirection.Normalize();

      tangentDirection = (-1f * Vector3.Cross(tensionDirection, pendulumSideDirection)).normalized;


      float inclinationAngle = Vector3.Angle(bob_p - pivot_p, gravityDirection);

      tensionForce = this.mass * Physics.gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad * inclinationAngle);
      float centripetalForce = ((this.mass * Mathf.Pow(this.currentVelocity.magnitude, 2)) / this.ropeLength);
      tensionForce += centripetalForce;

      this.currentVelocity += tensionDirection * tensionForce * deltaTime;
    }

    // Get the movement delta
    Vector3 movementDelta = this.currentVelocity * deltaTime;

    //return currentStatePosition + movementDelta;

    // This is probably what forces the ball on the edge
    float distance = Vector3.Distance(pivot_p, currentStatePosition + movementDelta);
    return this.GetPointOnLine(pivot_p, currentStatePosition + movementDelta, distance <= this.ropeLength ? distance : this.ropeLength);
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
    if (this.startingPositionSet)
      this.ResetBob(this.startingPosition);
    else
      this.PendulumInit();
  }

  // Use this to reset any built up forces
  [ContextMenu("Reset Pendulum Forces")]
  void ResetPendulumForces()
  {
    this.currentVelocity = Vector3.zero;

    // Set the transition state
    this.currentStatePosition = transform.position;
  }

  void PendulumInit()
  {
    // Get the initial rope length from how far away the bob is now
    this.ropeLength = Vector3.Distance(Pivot.transform.position, transform.position);

    this.currentStatePosition = transform.position;
    //this.ResetPendulumForces();
  }

  void ResetBob(Vector3 resetBobPosition)
  {
    // Put the bob back in the place we first saw it at in `Start()`
    transform.position = resetBobPosition;

    // Set the transition state
    this.currentStatePosition = resetBobPosition;
  }
  */
  #endregion

/*
  void OnDrawGizmos()
  {
    // purple
    Gizmos.color = new Color(.5f, 0f, .5f);
    Gizmos.DrawWireSphere(this.Pivot.transform.position, this.ropeLength);

    Gizmos.DrawWireCube(this.bobStartingPosition, new Vector3(.5f, .5f, .5f));


    // Blue: Auxilary
    Gizmos.color = new Color(.3f, .3f, 1f); // blue
    Vector3 auxVel = .3f * this.currentVelocity;
    Gizmos.DrawRay(this.Bob.transform.position, auxVel);
    Gizmos.DrawSphere(this.Bob.transform.position + auxVel, .2f);

    // Yellow: Gravity
    Gizmos.color = new Color(1f, 1f, .2f);
    Vector3 gravity = .3f * this.gravityForce * this.gravityDirection;
    Gizmos.DrawRay(this.Bob.transform.position, gravity);
    Gizmos.DrawSphere(this.Bob.transform.position + gravity, .2f);

    // Orange: Tension
    Gizmos.color = new Color(1f, .5f, .2f); // Orange
    Vector3 tension = .3f * this.tensionForce * this.tensionDirection;
    Gizmos.DrawRay(this.Bob.transform.position, tension);
    Gizmos.DrawSphere(this.Bob.transform.position + tension, .2f);

    // Red: Resultant
    Gizmos.color = new Color(1f, .3f, .3f); // red
    Vector3 resultant = gravity + tension;
    Gizmos.DrawRay(this.Bob.transform.position, resultant);
    Gizmos.DrawSphere(this.Bob.transform.position + resultant, .2f);


    ////////
    // Green: Pendulum side direction
    Gizmos.color = new Color(.3f, 1f, .3f);
    Gizmos.DrawRay(this.Bob.transform.position, 3f*this.pendulumSideDirection);
    Gizmos.DrawSphere(this.Bob.transform.position + 3f*this.pendulumSideDirection, .2f);
    ////////

    ////////
    // Cyan: tangent direction
    Gizmos.color = new Color(.2f, 1f, 1f); // cyan
    Gizmos.DrawRay(this.Bob.transform.position, 3f*this.tangentDirection);
    Gizmos.DrawSphere(this.Bob.transform.position + 3f*this.tangentDirection, .2f);
    ////////
  }
*/
}