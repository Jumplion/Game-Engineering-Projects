using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerSounds : MonoBehaviour {

  AudioSource audioSrc;
  public AudioClip pickaxeHit;
  public float hitRate = 1.0f;
  public float pitchVariance = 0.1f;
  float nextUpdate;

  private void Awake()
  {
    audioSrc = GetComponent<AudioSource>();
    audioSrc.loop = false;
    audioSrc.clip = pickaxeHit;
  }

  void OnDigHit()
  {
    if (nextUpdate > Time.time)
      return;

    nextUpdate = Time.time + hitRate;

    audioSrc.pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);

    audioSrc.Play();
  }

}
