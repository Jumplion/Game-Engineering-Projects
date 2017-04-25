using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float playerSpeed = 10.0f;
  public Vector3 gravity = new Vector3(0, -10, 0);
  public float jumpHeight = 2.0f;

  public LayerMask collisionMask;
  public LayerMask groundMask;

  public Vector3 speed;
  public Vector3 inputVector;
  public bool grounded = false;

  private float sphereRadius = 0.5f;
  private float groundBuffer = 0.1f;

  //private GrapplingHook hook;
  private Pendulum hook;

  private void Awake()
  {
    sphereRadius = GetComponent<SphereCollider>().radius;
    //groundBuffer = GetComponent<SphereCollider>().radius;
    hook = GetComponent<Pendulum>();
    hook.enabled = false;

    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update()
  {
    //Vector3 buffer = new Vector3(transform.position.x, transform.position.y - groundBuffer, transform.position.z);
    //Debug.DrawLine(transform.position, buffer, Color.red);
  }

  private void FixedUpdate()
  {
    // Get the input vector from the player
    Vector3 forwardVect = transform.forward * Input.GetAxis("Vertical");
    Vector3 sideVect = transform.right * Input.GetAxis("Horizontal");
    inputVector = (forwardVect + sideVect) * playerSpeed;

    // Check if the player is on the ground right now
    RaycastHit hit;
    grounded = Physics.SphereCast(new Ray(transform.position, -transform.up), sphereRadius, out hit, groundBuffer + 0.001f, groundMask);

    Vector3 endPosition = transform.position;
    endPosition += inputVector * Time.deltaTime;

    // If the player is grounded, and not tethered, we just want regular controls.
    if (grounded)
    {
      endPosition += (hit.point - transform.position).normalized * (hit.distance - 0.001f);

      if (Input.GetButtonDown("Jump"))
        endPosition += new Vector3(0, Mathf.Sqrt(2 * jumpHeight * -gravity.y), 0);
    }
    else
    {
      endPosition += gravity * Time.deltaTime;
    }

    // Go to the test position
    // speed = (endPosition - transform.position) / Time.deltaTime;
    // transform.position = transform.position * speed * Time.deltaTime;
    transform.position = endPosition;
  }
}