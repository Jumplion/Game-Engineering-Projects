using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject optionsMenu;
  public GameObject creditsMenu;

  public void StartGamePressed()
  {
    SceneManager.LoadScene(1, LoadSceneMode.Single);
  }
  	
  public void OptionsPressed()
  {
    SwapToMenu(optionsMenu);
  }

  public void CreditsPressed()
  {
    SwapToMenu(creditsMenu);
  }

  private void SwapToMenu(GameObject otherMenu)
  {
    otherMenu.SetActive(true);
    gameObject.SetActive(false);
  }

  public void ExitPressed()
  {
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
  }

}
