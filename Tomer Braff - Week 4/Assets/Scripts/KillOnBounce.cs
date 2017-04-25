using UnityEngine;

public class KillOnBounce : MonoBehaviour {

	void OnBounce(Ball b)
	{
		Destroy(b.gameObject);
	}
}
