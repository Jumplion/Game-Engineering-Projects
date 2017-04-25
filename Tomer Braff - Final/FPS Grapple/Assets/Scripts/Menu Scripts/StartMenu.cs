using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
  public GameObject mainMenu;

  public Button amazingGameButton;
  public Text amazingGameButtonText;

  public string[] amazingGameButtonPhrases;

  public void BackPressed()
  {
    mainMenu.SetActive(true);
    gameObject.SetActive(false);
  }

  //************************************
  // TODO: INSERT START MENU LOGIC
  //************************************

  public void AmazingGameButtonPressed()
  {
    amazingGameButton.transform.localPosition = new Vector2(Random.Range(-400, 400), Random.Range(-400, 400));
    amazingGameButton.transform.Rotate(0, 0, Random.Range(0, 360));
    amazingGameButtonText.text = amazingGameButtonPhrases[Random.Range(0, amazingGameButtonPhrases.Length - 1)];
  }
}
