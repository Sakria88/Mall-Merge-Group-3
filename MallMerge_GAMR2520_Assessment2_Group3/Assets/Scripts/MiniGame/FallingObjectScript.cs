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
        // We warn the pooler that this object should be returned to the pool
        OnReturnToPool?.Invoke(gameObject);
    }
}
