using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
  public GameObject pickup;
  public int numToSpawn = 10;

  void OnBlockDestroyed()
  {
    for(int x=0; x < numToSpawn; x++)
      Instantiate(pickup, transform.position, Quaternion.LookRotation(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
  }
}
