using UnityEngine;

public class MaterialSwap : MonoBehaviour
{
  public Gradient colorGradient;
  public float minResetTime = 0.05f;
  public float maxResetTime = 0.15f;
  float nextUpdate;

  void OnBlockDamaged()
  {
    if (nextUpdate > Time.time)
      return;

    nextUpdate = Time.time + Mathf.Lerp(minResetTime, maxResetTime, Random.value);

    Material mat = GetComponent<Renderer>().material;

    float healthPercent = GetComponent<DestructableBlock>().GetPercentRemaining();
    mat.color = colorGradient.Evaluate(healthPercent);

    // Default way to set a material parameter color thing
    // mat.SetFloat("WarpStrength", Random.value);
  }
}
