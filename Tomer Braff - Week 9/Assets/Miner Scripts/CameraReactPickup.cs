using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReactPickup : MonoBehaviour
{
  public float reactSeverity = 2.0f;
  public float reactDuration = 0.5f;
  private float originalFOV;

  delegate void dlgt_OnCamReact();
  static event dlgt_OnCamReact evnt_OnCamReact;

  private void Awake()
  {
    originalFOV = Camera.main.fieldOfView;
    evnt_OnCamReact += ReactToPickup;
  }

  public static void ReactFunction()
  {
    evnt_OnCamReact();
  }

  void ReactToPickup()
  {
    StartCoroutine("React");
  }
  
  IEnumerator React()
  {
    Camera cam = Camera.main;
    float timeCompleted = reactDuration;

    while(timeCompleted > 0)
    {
      float percentage = timeCompleted / reactDuration;

      cam.fieldOfView = originalFOV + (percentage * reactSeverity);

      timeCompleted -= Time.deltaTime;
      yield return new WaitForSeconds(Time.deltaTime);
    }

    cam.fieldOfView = originalFOV;
  }
}
