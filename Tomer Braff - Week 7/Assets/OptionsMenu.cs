using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

  public GameObject mainMenu;

  public Dropdown resolutionDropdown;
  public Toggle windowedToggle;
  public Toggle invertYAxisToggle;
  public Slider mouseSensitivitySlider;
  public Slider masterVolumeSlider;
  public Slider gameVolumeSlider;
  public Slider musicVolumeSlider;

  public void Start()
  {
    NotificationCenter.Default.AddObserver("LoadResolution", LoadResolution);
    NotificationCenter.Default.AddObserver("LoadWindowed", LoadWindowed);
    NotificationCenter.Default.AddObserver("LoadInvertY", LoadInvertY);
    NotificationCenter.Default.AddObserver("LoadMouseSensitivity", LoadMouseSensitivity);
    NotificationCenter.Default.AddObserver("LoadMasterVolume", LoadMasterVolume);
    NotificationCenter.Default.AddObserver("LoadGameVolume", LoadGameVolume);
    NotificationCenter.Default.AddObserver("LoadMusicVolume", LoadMusicVolume);
  }

  public void BackPressed()
  {
    ReturnToMainMenu();
  }

  public void ConfirmPressed()
  {
    // Save all the current settings of the options
    // Then return to the main menu

    NotificationCenter.Default.PostNotification("SaveResolution", resolutionDropdown.value);
    NotificationCenter.Default.PostNotification("SaveWindowed", windowedToggle.isOn);
    NotificationCenter.Default.PostNotification("SaveInvertY", invertYAxisToggle.isOn);
    NotificationCenter.Default.PostNotification("SaveMouseSensitivity", mouseSensitivitySlider.value);
    NotificationCenter.Default.PostNotification("SaveMasterVolume", masterVolumeSlider.value);
    NotificationCenter.Default.PostNotification("SaveGameVolume", gameVolumeSlider.value);
    NotificationCenter.Default.PostNotification("SaveMusicVolume", musicVolumeSlider.value);

    ReturnToMainMenu();
  }

  void ReturnToMainMenu()
  {
    mainMenu.SetActive(true);
    gameObject.SetActive(false);
  }

  //************************************
  // TODO: INSERT START MENU LOGIC
  //************************************

  // When the option menu first enables, 
  private void OnEnable()
  {
    // Now call for the settings of each
    NotificationCenter.Default.PostNotification("LoadResolution");
    NotificationCenter.Default.PostNotification("LoadWindowed");
    NotificationCenter.Default.PostNotification("LoadInvertY");
    NotificationCenter.Default.PostNotification("LoadMouseSensitivity");
    NotificationCenter.Default.PostNotification("LoadMasterVolume");
    NotificationCenter.Default.PostNotification("LoadGameVolume");
    NotificationCenter.Default.PostNotification("LoadMusicVolume");
  }

  void LoadResolution(object defaultValue)
  {
    int dropdownIndex = PlayerPrefs.GetInt("Resolution", 0);
    resolutionDropdown.value = dropdownIndex;
  }
  void LoadWindowed(object defaultValue)
  {
    bool isWindowed = PlayerPrefs.GetInt("Windowed", 0) == 1;
    windowedToggle.isOn = isWindowed;
  }
  void LoadInvertY(object defaultValue)
  {
    bool isInverted = PlayerPrefs.GetInt("InvertY", 0) == 1;
    invertYAxisToggle.isOn = isInverted;
  }
  void LoadMouseSensitivity(object defaultValue)
  {
    float sliderValue = PlayerPrefs.GetFloat("MouseSensitivity", 0);
    mouseSensitivitySlider.value = sliderValue;
  }
  void LoadMasterVolume(object defaultValue)
  {
    float sliderValue = PlayerPrefs.GetFloat("MasterVolume", 0);
    masterVolumeSlider.value = sliderValue;
  }
  void LoadGameVolume(object defaultValue)
  {
    float sliderValue = PlayerPrefs.GetFloat("GameVolume", 0);
    gameVolumeSlider.value = sliderValue;
  }
  void LoadMusicVolume(object defaultValue)
  {
    float sliderValue = PlayerPrefs.GetFloat("MusicVolume", 0);
    musicVolumeSlider.value = sliderValue;
  }
}
