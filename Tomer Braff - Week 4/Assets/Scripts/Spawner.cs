using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	public GameObject ballPrefab;
	public int numBalls = 10;
	public Text numBallText;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0) && numBalls > 0)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos = new Vector3(mousePos.x, mousePos.y);

			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
        if(hit.transform.name == "Spawner")
				{
					numBalls--;
					Instantiate(ballPrefab, mousePos, Quaternion.identity);
				}
		}

		numBallText.text = "Balls\n" + numBalls;
	}
}
