using UnityEngine;
using System;

public class FallingObjectScript : MonoBehaviour
{
    public float fallSpeed = 3f;

    // Callback al pooler
    public Action<GameObject> OnReturnToPool;

    protected virtual void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Basket"))
        {
            OnCollected();
        }
    }

    protected virtual void OnCollected()
    {
        
        OnReturnToPool?.Invoke(gameObject);
    }
}
