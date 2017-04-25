using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
  public static Color[] colors = { Color.red, Color.green, Color.blue, Color.black, Color.yellow, Color.magenta, Color.white, Color.grey, Color.cyan };
  public float spawnForce = 5.0f;

  public static GameObject miner;
  public static float pickupDistance = 1.0f;
  public static float pickupSpeed = 5.0f;

  private Rigidbody rb;
  bool pickedUp = false;

  private void Awake()
  {
    miner = GameObject.FindGameObjectWithTag("Miner");
    rb = GetComponent<Rigidbody>();
  }

	// Use this for initialization
	void Start ()
  {
    GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Length - 1)];

    rb.AddForce(transform.forward * spawnForce);
	}

  void Update()
  {
    if (Vector3.Distance(transform.position, miner.transform.position) <= pickupDistance && !pickedUp)
    {
      pickedUp = true;
      Destroy(rb);
      StartCoroutine("GoTowardsPlayer", miner.transform);
    }
  }

  IEnumerator GoTowardsPlayer(Transform playerTransform)
  {
    Destroy(rb);

    while (true)
    {
      Vector3 direction = (playerTransform.position - transform.position).normalized;

      transform.position += direction * pickupSpeed * Time.deltaTime;

      yield return new WaitForSeconds(Time.deltaTime);

      if (Vector3.Distance(playerTransform.position, transform.position) <= 0.1f)
      {
        CameraReactPickup.ReactFunction();
        Destroy(gameObject);
      }
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if(collision.gameObject.tag == "Miner")
      Destroy(rb);
  }
}
