using UnityEngine;

public class BasketController : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        float move = Input.acceleration.x;
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Energy"))
            GameManager.Instance.Score.Add(1);

        else if (col.CompareTag("Energy_5"))
            GameManager.Instance.Score.Add(5);

        else if (col.CompareTag("Energy_15"))
            GameManager.Instance.Score.Add(15);

        else if (col.CompareTag("Bomb"))
            GameManager.Instance.EndGame(false);
    }
}
