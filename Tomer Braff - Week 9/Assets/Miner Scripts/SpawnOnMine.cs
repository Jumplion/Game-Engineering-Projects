using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnMine : MonoBehaviour
{
  public GameObject prefab;
  public float lifetime = 3.0f;
  public float mineRate = 0.5f;
  private float nextMineTime;

  void OnDigHit()
  {
    if (nextMineTime > Time.time)
      return;

    nextMineTime = Time.time + mineRate;

    RaycastHit mineHit = GetComponent<BlockDigger>().GetLastHit();
    GameObject newGO = Instantiate(prefab, mineHit.point, Quaternion.LookRotation(mineHit.normal));

    Destroy(newGO, lifetime);
  }
}
