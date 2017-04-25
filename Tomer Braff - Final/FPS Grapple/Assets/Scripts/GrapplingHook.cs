using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
  public LayerMask grappleLayerMask;
  public float maxDistance = 100;

  private bool tethered = false;
  public bool Tethered { get { return tethered; } }

  private GameObject grappledObject;
  public GameObject GrappleObject { get { return grappledObject; } }
  private Vector3 point;
  public Vector3 Point { get { return point; } }
  private float length;
  public float Length { get { return length; } }

  void Update()
  {
    // Left Mouse button down, throw a raycast forward
    if (Input.GetMouseButtonDown(0))
    {
      RaycastHit hit;

      if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, grappleLayerMask))
      {
        grappledObject = hit.transform.gameObject;
        point = grappledObject.transform.position;
        length = hit.distance;

        tethered = true;
      }
      else
      {
        tethered = false;
      }
    }   
  }

  void RetractHook()
  {

  }
}
