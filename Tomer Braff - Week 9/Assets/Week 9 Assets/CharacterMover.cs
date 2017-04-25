using UnityEngine;
using System.Collections;

public class CharacterMover : MonoBehaviour
{
    public CharacterController character;
    public Transform playerCamera;

    public Vector3 velocity;
    public Vector3 acceleration;

    public float accelRate = 8f;
    public float maxSpeed = 2f;
    public float jumpStrength = 4f;
    public float walkFriction = 20f;

    public float floorCheckDist = 0.1f;
    public float minFloorAngle = 0.8f;

    public Vector3 gravity = new Vector3(0f, -9.8f, 0f);
  
	void Update ()
    {
        ApplyAcceleration();
        ClampHorizontalVelocity();

        bool onGround = CheckForGround();
        if (onGround)
        {
            ApplyFriction();
            CheckJump();
        }
        else
            ApplyGravity();

        // apply velocity
        character.Move(velocity * Time.deltaTime);
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Dampen any velocity towards the impact surface
        velocity = velocity + (hit.normal * Vector3.Dot(-hit.normal, velocity));
    }

    void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
            velocity.y = jumpStrength;
    }

    void ApplyGravity()
    {
        velocity += gravity * Time.deltaTime;
    }

    void ApplyAcceleration()
    {
        // Find the players movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        acceleration.x = x;
        acceleration.z = z;

        // Rotate the movement from relative to world orientation, along the ground
        Vector3 cameraDir = playerCamera.forward;
        cameraDir.y = 0f;
        cameraDir = cameraDir.normalized;

        acceleration = Quaternion.LookRotation(cameraDir) * acceleration;

        // apply horizontal acceleration
        velocity += acceleration * accelRate * Time.deltaTime;
    }

    void ClampHorizontalVelocity()
    {
        Vector3 horizVel = velocity;
        horizVel.y = 0f;
        if (horizVel.magnitude > maxSpeed)
            horizVel = horizVel.normalized * maxSpeed;
        velocity.x = horizVel.x;
        velocity.z = horizVel.z;
        // Ignore the y value to prevent clamping the fall speed
    }

    void ApplyFriction()
    {
        Vector3 horizVel = velocity;
        horizVel.y = 0f;

        Vector3 frictionDir = velocity.normalized;
        if (acceleration.sqrMagnitude > 0.001f)
        {
            // Don't apply friction along the acceleration direction
            frictionDir += acceleration.normalized * Vector3.Dot(-acceleration.normalized, frictionDir);
        }

        // Apply friction only when on the ground
        Vector3 frameFriction = frictionDir * walkFriction * Time.deltaTime;
        if (frameFriction.magnitude > horizVel.magnitude)
            horizVel = Vector3.zero;
        else
            horizVel -= frameFriction;

        velocity.x = horizVel.x;
        velocity.z = horizVel.z;
    }

    bool CheckForGround()
    {
        if (velocity.y > 0f)
            return false;
        
        Vector3 p1 = transform.position + character.center + Vector3.down * (character.height / 2f - character.radius);
        Vector3 p2 = transform.position + character.center + Vector3.up * (character.height / 2f - character.radius);
        
        RaycastHit hitInfo;
        if(Physics.CapsuleCast(p1,p2,character.radius, Vector3.down, out hitInfo, floorCheckDist))
        {
            return Vector3.Dot(Vector3.up, hitInfo.normal) > minFloorAngle;
        }
        return false;
    }
}
