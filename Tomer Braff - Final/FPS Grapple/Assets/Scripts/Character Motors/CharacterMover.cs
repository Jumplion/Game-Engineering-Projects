using UnityEngine;

public enum MotorState
{
  start,
  walking,
  falling,
  grappling,
  wallRunning
}

public class CharacterMover : MonoBehaviour
{
  public Vector3 gravity = new Vector3(0, -10, 0);
  public LayerMask collisionMask;
  public float grappleSphereCastRadius = 2.0f;
  public float jumpHeight = 2.0f;

  public Animator UIAnimator;

  public float maxStreakSpeed;

  private WalkingMotor walkingMotor;
  private FallingMotor fallingMotor;
  private StartMotor startMotor;
  private GrappleMotor grappleMotor;
  private WallRunMotor wallRunMotor;

  [System.NonSerialized] public SpeedStreaks streaks;
  [System.NonSerialized] public CharacterController controller;
  [System.NonSerialized] public Vector3 velocity;
  [System.NonSerialized] public Transform resetLocation;
  [System.NonSerialized] public MotorState currentState;
  [System.NonSerialized] public Vector3 inputVector;

  public AudioClip windSFX;
  private AudioSource audioSrc;

  private void Awake()
  {
    controller = GetComponent<CharacterController>();
    walkingMotor = GetComponent<WalkingMotor>();
    fallingMotor = GetComponent<FallingMotor>();
    startMotor = GetComponent<StartMotor>();
    grappleMotor = GetComponent<GrappleMotor>();
    wallRunMotor = GetComponent<WallRunMotor>();

    audioSrc = GetComponent<AudioSource>();

    resetLocation = GameObject.Find("Reset Location").transform;
    streaks = GameObject.Find("Speed Streaks").GetComponent<SpeedStreaks>();

    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update()
  {
    // Get the input vector from the player
    PlayerInput();

    switch (currentState)
    {
      case MotorState.start:
        startMotor.UpdateMotor(this);
        break;
      case MotorState.walking:
        walkingMotor.UpdateMotor(this);
        break;
      case MotorState.falling:
        fallingMotor.UpdateMotor(this);
        break;
      case MotorState.grappling:
        grappleMotor.UpdateMotor(this);
        break;
      case MotorState.wallRunning:
        wallRunMotor.UpdateMotor(this);
        break;
    }

    if (currentState == MotorState.falling || currentState == MotorState.grappling)
    {
      streaks.gameObject.SetActive(true);
      HandleParticles();

      if (audioSrc.clip != windSFX)
        audioSrc.clip = windSFX;

      if (!audioSrc.isPlaying)
        audioSrc.Play();

      audioSrc.volume = velocity.magnitude / maxStreakSpeed;
    }
    else
    {
      streaks.gameObject.SetActive(false);
      audioSrc.Stop();
    }

    if (currentState != MotorState.grappling)
    {
      controller.Move(velocity * Time.deltaTime);

      if (CheckForFloor())
        currentState = MotorState.falling;
    }
  }

  private void OnControllerColliderHit(ControllerColliderHit hit)
  {
    switch (currentState)
    {
      case MotorState.start:
        startMotor.HandleCollision(this, hit);
        break;
      case MotorState.walking:
        walkingMotor.HandleCollision(this, hit);
        break;
      case MotorState.falling:
        fallingMotor.HandleCollision(this, hit);
        break;
      case MotorState.grappling:
        grappleMotor.HandleCollision(this, hit);
        break;
    }
  }

  public void PlayerInput()
  {
    // Get Arrow Keys/WASD
    Vector3 forwardVect = transform.forward * Input.GetAxis("Vertical");
    Vector3 sideVect = transform.right * Input.GetAxis("Horizontal");
    inputVector = (forwardVect + sideVect);

    CheckGrapple();

    if (currentState == MotorState.falling)
      HandleWallRun();
  }

  private void CheckGrapple()
  {
    RaycastHit hit;
    bool inRange = Physics.SphereCast(transform.position, grappleSphereCastRadius, Camera.main.transform.forward, out hit, grappleMotor.maxRopeLength, grappleMotor.grappleLayer);
      
    UIAnimator.SetBool("InRange", inRange);

      // Check if the player wants to grapple
    if (Input.GetMouseButtonDown(0) && inRange)
    {
      grappleMotor.InitializeGrapple(hit.transform.gameObject);
      audioSrc.PlayOneShot(grappleMotor.grappleSFX);
      currentState = MotorState.grappling;
    }
    // Let go of left click, no more grappling
    else if (Input.GetMouseButtonUp(0))
    {
      grappleMotor.DisableGrappleUI();
      currentState = MotorState.falling;
    }
  }

  private void HandleWallRun()
  {
    RaycastHit wallHit;
    if (CheckCollision(transform.right, out wallHit))
    {
      // Pass the hit point's normal to the wallrun motor
      wallRunMotor.wallHit = wallHit;
      wallRunMotor.wallRunForward = Vector3.Cross(Vector3.up, wallHit.normal);

      currentState = MotorState.wallRunning;
    }
    else if (CheckCollision(-transform.right, out wallHit))
    {
      // Pass the hit point's normal to the wallrun motor
      wallRunMotor.wallHit = wallHit;
      wallRunMotor.wallRunForward = Vector3.Cross(wallHit.normal, Vector3.up);
      currentState = MotorState.wallRunning;
    }
  }

  private void HandleParticles()
  {
    if (velocity.magnitude / maxStreakSpeed < 0.25)
      streaks.ActivateParticleSystem(-1);
    else if (velocity.magnitude / maxStreakSpeed < 0.5)
      streaks.ActivateParticleSystem(0);
    else if (velocity.magnitude / maxStreakSpeed < 0.75)
      streaks.ActivateParticleSystem(1);
    else
      streaks.ActivateParticleSystem(2);
  }

  private bool CheckCollision(Vector3 direction, out RaycastHit wallHit)
  {
    // Check if the player is wall running
    return Physics.Raycast(transform.position, direction, out wallHit, 1 + wallRunMotor.wallBuffer, wallRunMotor.wallRunLayer);
  }

  public bool CheckCollision(out RaycastHit hit, Vector3 moveDelta)
  {
    return Physics.SphereCast(transform.position, 1.0f, moveDelta.normalized, out hit, moveDelta.magnitude, collisionMask);
  }

  public bool CheckForFloor()
  {
    RaycastHit hitInfo;
    if (Physics.SphereCast(transform.position, 1.0f, Vector3.down, out hitInfo, 0.2f, collisionMask))
      return true; //Vector3.Dot(Vector3.up, hitInfo.normal) > 0.7f;

    return false;
  }

  public bool CheckForFloor(out RaycastHit hit)
  {
    if (Physics.SphereCast(transform.position, 1.0f, Vector3.down, out hit, 0.2f, collisionMask))
      return true;//Vector3.Dot(Vector3.up, hit.normal) > 0.7f;

    return false;
  }

  public void ResetPlayer()
  {
    currentState = MotorState.falling;
    velocity = Vector3.zero;
    transform.position = resetLocation.position;
  }

  public float GetMaxGrappleDistance()
  {
    if(!grappleMotor)
      grappleMotor = GetComponent<GrappleMotor>();

    return grappleMotor.maxRopeLength;
  }
}