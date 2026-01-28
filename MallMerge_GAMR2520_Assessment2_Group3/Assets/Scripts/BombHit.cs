using UnityEngine;

public class BombHit : MonoBehaviour
{
    private GameObject explosionPrefab;
    private Vector3 explosionOffset = Vector3.zero;

    private string basketTag = "Basket";

    private bool endGameOnHit = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(basketTag)) return;

        TriggerExplosion();
    }
    private void TriggerExplosion()
    {
        hasTriggered = true;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position + explosionOffset, Quaternion.identity);
        }

        if (endGameOnHit && GameManager.Instance != null)
        {
            GameManager.Instance.EndGame(false);
        }

        Destroy(gameObject);
    }
}
