using UnityEngine;

public class BombHit : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Vector3 explosionOffset = Vector3.zero;

    [Header("Collision")]
    [SerializeField] private string basketTag = "Basket";

    [Header("Gameplay (optional)")]
    [SerializeField] private bool endGameOnHit = true;

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
