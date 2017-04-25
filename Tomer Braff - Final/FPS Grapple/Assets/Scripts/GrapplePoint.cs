using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
  public static GameObject player;
  public static CharacterMover playerMover;
  public static float maxDistance = 25.0f;
  public Gradient grappleState;
  public Color grappleableColor;

  private Material mat;

	// Use this for initialization
	void Awake ()
  {
    mat = GetComponent<Renderer>().material;
    if(player == null)
    {
      player = GameObject.FindGameObjectWithTag("Player");
      playerMover = player.GetComponent<CharacterMover>();
      maxDistance = playerMover.GetMaxGrappleDistance();
    }
	}

  // Update is called once per frame
  void Update()
  {
    float distance = Vector3.Distance(transform.position, player.transform.position);

    if (distance < playerMover.GetMaxGrappleDistance())
      mat.color = grappleableColor;
    else
      mat.color = grappleState.Evaluate(1 - distance / maxDistance);
	}

  void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, maxDistance);
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, maxDistance);
  }
}
