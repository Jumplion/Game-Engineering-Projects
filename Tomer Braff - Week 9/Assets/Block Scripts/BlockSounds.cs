using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSounds : MonoBehaviour
{
  public float pitchVariance = 0.1f;

  private void Awake()
  {
    GetComponent<AudioSource>().pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);

    Destroy(gameObject, 1.0f);
  }
}
