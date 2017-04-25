using UnityEngine;

public class SpeedStreaks : MonoBehaviour
{
  public GameObject[] particleSystems;

  private int currentSysIndex = -1;

  private void Awake()
  {
    for (int x = 0; x < particleSystems.Length; x++)
      particleSystems[x].SetActive(false);
  }

  public void ActivateParticleSystem(int sysIndex)
  {
    if(currentSysIndex >= 0 && currentSysIndex < particleSystems.Length)
      particleSystems[currentSysIndex].SetActive(false);
      
    if(sysIndex >= 0 && sysIndex < particleSystems.Length)
      particleSystems[sysIndex].SetActive(true);

    currentSysIndex = sysIndex;
  }
}
