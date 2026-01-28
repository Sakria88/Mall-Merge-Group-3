using UnityEngine;

public class BombObjectScript : FallingObjectScript
{
    private int energyValue = -5;
    private GameObject explosionPrefab;   
    private Vector3 explosionOffset = Vector3.zero;

    private bool hasExploded = false;

    protected override void OnCollected()
    {
        if (hasExploded) return;
        hasExploded = true;

        Debug.Log("Boooooom! Value: " + energyValue);

    
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position + explosionOffset, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("BombObjectScript: explosionPrefab is NOT assigned on " + gameObject.name);
        }

        // Return bomb to pool / existing collected logic
        base.OnCollected();
    }
}
