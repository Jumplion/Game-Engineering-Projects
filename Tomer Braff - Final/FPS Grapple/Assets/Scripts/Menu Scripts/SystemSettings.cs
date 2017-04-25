using UnityEngine;

public class SystemSettings : MonoBehaviour
{
  private void Awake()
  {
    NotificationCenter.Default.AddObserver("SaveResolution", SaveResolution);
    NotificationCenter.Default.AddObserver("SaveWindowed", SaveWindowed);
    NotificationCenter.Default.AddObserver("SaveInvertY", SaveInvertY);
    NotificationCenter.Default.AddObserver("SaveMouseSensitivity", SaveMouseSensitivity);
    NotificationCenter.Default.AddObserver("SaveMasterVolume", SaveMasterVolume);
    NotificationCenter.Default.AddObserver("SaveGameVolume", SaveGameVolume);
    NotificationCenter.Default.AddObserver("SaveMusicVolume", SaveMusicVolume);
  }

  void SaveResolution(object value)
  {
    PlayerPrefs.SetInt("Resolution", (int)value);
    PlayerPrefs.Save();
  }

  void SaveWindowed(object value)
  {
    PlayerPrefs.SetInt("Windowed", (bool)value ? 1 : 0);
    PlayerPrefs.Save();
  }

  void SaveInvertY(object value)
  {
    PlayerPrefs.SetInt("InvertY", (bool)value ? 1 : 0);
    PlayerPrefs.Save();
  }

  void SaveMouseSensitivity(object value)
  {
    PlayerPrefs.SetFloat("MouseSensitivity", (float)value);
    PlayerPrefs.Save();
  }

  void SaveMasterVolume(object value)
  {
    PlayerPrefs.SetFloat("MasterVolume", (float)value);
    PlayerPrefs.Save();
  }

  void SaveGameVolume(object value)
  {
    PlayerPrefs.SetFloat("GameVolume", (float)value);
    PlayerPrefs.Save();
  }

  void SaveMusicVolume(object value)
  {
    PlayerPrefs.SetFloat("MusicVolume", (float)value);
    PlayerPrefs.Save();
  }
}
