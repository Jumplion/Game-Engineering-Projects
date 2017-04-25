using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
  public void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Player")
      other.GetComponent<CharacterMover>().ResetPlayer();
  }

  public void OnTriggerStay(Collider other)
  {
    if (other.tag == "Player")
      other.GetComponent<CharacterMover>().ResetPlayer();
  }
}
