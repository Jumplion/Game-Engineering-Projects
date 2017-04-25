using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeSwing : MonoBehaviour
{
  Animator anim;

	// Use this for initialization
	void Start ()
  {
    anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
  {
    if (Input.GetButton("Fire1"))
    {
      anim.SetTrigger("Swing");
    }

    if (Input.GetButtonUp("Fire1"))
    {
      anim.ResetTrigger("Swing");
    }
  }
}
