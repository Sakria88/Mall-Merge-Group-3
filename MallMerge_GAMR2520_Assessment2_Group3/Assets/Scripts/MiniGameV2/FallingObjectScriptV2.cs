using UnityEngine;
using System;

public class FallingObjectScriptV2 : MonoBehaviour
{
    public float fallSpeed = 3f;
    public Action<GameObject> OnReturnToPool;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Basket"))
            OnReturnToPool?.Invoke(gameObject);
    }
}
