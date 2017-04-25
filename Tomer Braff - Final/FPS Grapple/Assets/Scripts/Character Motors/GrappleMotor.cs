using UnityEngine;

public class GrappleMotor : BaseMotor
{
  // The point where the game object swings around
  [System.NonSerialized] public GameObject Pivot;
  [System.NonSerialized] public float ropeLength = 5.0f;

  public AudioClip grappleSFX;
  public GameObject grappleArrow;
  public LayerMask grappleLayer;

  [Header("Rope Variables")]
  public float minRopeLength = 5.0f;
  public float maxRopeLength = 20.0f;
  public float retractSpeed = 10.0f;

  [Header("Player Variables")]
  public float inputPower = 5.0f;
  public float mass = 1.0f;
  public float maxSpeed = 50.0f;
  public float snapBackPower = 1.0f;

  [Header("Boosting Variables")]
  public float boostPower = 5.0f;
  public float boostCooldown = 2.0f;

  private float boostDelta = 0;
  private bool boosting = false;

  private Transform hand;
  private LineRenderer lr;

  // We use these to smooth between values in certain framerate situations in the `Update()` loop
  [System.NonSerialized] public Vector3 currentStatePosition;
  private Vector3 tempVelocity;

  public void Awake()
  {
    lr = GetComponent<LineRenderer>();
    hand = GameObject.Find("Hand").transform;
  }

  public override void UpdateMotor(CharacterMover mover)
  {
    mover.currentState = MotorState.grappling;

    lr.SetPosition(0, hand.position);
    lr.SetPosition(1, Pivot.transform.position);

    tempVelocity = mover.velocity;

    // If the player is right clicking as they're swinging, retract the rope
    if (Input.GetMouseButton(1))
      ropeLength -= retractSpeed * Time.deltaTime;

    if (boosting = ((boostDelta -= Time.deltaTime) <= 0 && Input.GetKeyDown(KeyCode.Space)))
      boostDelta = boostCooldown;


    ropeLength = Mathf.Clamp(ropeLength, minRopeLength, maxRopeLength);

    currentStatePosition = PendulumUpdate(transform.position, Time.deltaTime, mover);
    tempVelocity = tempVelocity.magnitude > maxSpeed ? tempVelocity.normalized * maxSpeed : tempVelocity;
    mover.velocity = tempVelocity;


    UpdateGrappleArrow();


    transform.position = currentStatePosition;
  }

  Vector3 PendulumUpdate(Vector3 startPos, float deltaTime, CharacterMover mover)
  {
    float gravMagnitude = mover.gravity.magnitude;
    Vector3 gravDirection = mover.gravity.normalized;
    Vector3 playerInput = mover.inputVector;
    Vector3 pivotPosition = Pivot.transform.position;

    // Add gravity free fall (possibly also add player input)
    float pow = inputPower + (boosting ? boostPower : 0);

    float gravityForce = mass * gravMagnitude;
    tempVelocity += (((playerInput * pow) + (gravDirection * gravityForce)) * deltaTime);

    // How much gravity has affected us
    Vector3 auxiliaryMovementDelta = tempVelocity * deltaTime;
    float distanceAfterGravity = Vector3.Distance(pivotPosition, startPos + auxiliaryMovementDelta);

    // Did gravity push us past our rope length?
    if (distanceAfterGravity > ropeLength || Mathf.Approximately(distanceAfterGravity, ropeLength))
    {
      // Tension of the rope
      Vector3 tensionDirection = (pivotPosition - startPos).normalized;
      float inclinationAngle = Vector3.Angle(startPos - pivotPosition, gravDirection);
      float tensionForce = mass * gravMagnitude * Mathf.Cos(Mathf.Deg2Rad * inclinationAngle);
      float centripetalForce = ((mass * Mathf.Pow(tempVelocity.magnitude, 2)) / ropeLength);

      tensionForce += centripetalForce;

      // Add the rope tension force to the velocity
      tempVelocity += tensionDirection * tensionForce * deltaTime;
    }

    // Get the movement delta
    Vector3 movementDelta = tempVelocity * deltaTime;
    float distance = Vector3.Distance(pivotPosition, startPos + movementDelta);
    return GetPointOnLine(pivotPosition, startPos + movementDelta, distance <= ropeLength ? distance : ropeLength);
  }

  // Returns a vector a certain distance from the start of a given line
  Vector3 GetPointOnLine(Vector3 start, Vector3 end, float distanceFromStart)
  {
    return start + (distanceFromStart * Vector3.Normalize(end - start));
  }

  public void InitializeGrapple(GameObject piv)
  {
    Pivot = piv;
    ropeLength = Vector3.Distance(transform.position, Pivot.transform.position);
    lr.enabled = true;
    grappleArrow.SetActive(true);
  }

  public void DisableGrappleUI()
  {
    lr.enabled = false;
    grappleArrow.SetActive(false);
  }

  public void UpdateGrappleArrow()
  {
    Vector3 dir = Pivot.transform.position - currentStatePosition;
    float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
    grappleArrow.transform.localEulerAngles = new Vector3(0, 0, angle);
  }
}
