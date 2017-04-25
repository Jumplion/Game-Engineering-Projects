using UnityEngine;
using System.Collections;

public class DestructableBlock : MonoBehaviour
{
    public float blockHealth = 4f;
    float currentDamage;
    
    public void DamageBlock(float damageValue)
    {
        currentDamage += damageValue;
        
        if(currentDamage >= blockHealth)
        {
            // Block is destroyed, do cool stuff
            SendMessage("OnBlockDestroyed", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        else
        {
            SendMessage("OnBlockDamaged",SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ResetBlock()
    {
        currentDamage = 0f;
        SendMessage("OnBlockDamaged", SendMessageOptions.DontRequireReceiver);
    }

    // Returns a normalized amount of health.
    public float GetPercentRemaining()
    {
        return (blockHealth - currentDamage) / blockHealth;
    }

    // Returns the actual remaining health.
    public float GetRemainingHealth()
    {
        return blockHealth - currentDamage;
    }
}
