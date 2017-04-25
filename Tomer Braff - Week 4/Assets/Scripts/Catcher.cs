using UnityEngine;
using UnityEngine.UI;

public class Catcher : MonoBehaviour 
{
	public static int points = 0;
	public int pointsAwarded = 1;
	public Text pointText;

	void Start()
	{
		pointText.text = "Points\n" + points;
	}

	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "Ball")
		{
			Destroy(col.gameObject);
			points += pointsAwarded;

			pointText.text = "Points\n" + points;
		}
	}
}
