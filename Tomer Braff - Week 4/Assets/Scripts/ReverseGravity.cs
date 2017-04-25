using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravity : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Ball")
			col.SendMessage("ReverseGravity", null, SendMessageOptions.DontRequireReceiver);
	}

		void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Ball")
			col.SendMessage("ReverseGravity", null, SendMessageOptions.DontRequireReceiver);
	}
}
