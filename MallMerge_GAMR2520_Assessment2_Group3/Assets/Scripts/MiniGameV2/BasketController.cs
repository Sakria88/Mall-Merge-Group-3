using UnityEngine;

public class BasketController : MonoBehaviour
{
    public float movingSpeed = 5f;
    public float maxXSpeed = 5f;
    public float maxYSpeed = 15f;
    public Texture aTextureLeft;
    public Texture aTextureRight;

    Rigidbody2D rb;
    Camera cam;

    public enum InputMode { Touch, Accel }
    public InputMode inpMode = InputMode.Touch;

    float basketHalfWidth;
    float moveInput;
    float minX;
    float maxX;
    bool facingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();

        basketHalfWidth = GetComponent<Collider2D>().bounds.extents.x * 2;

        //playerBounceSFX = GameObject.Find("PlayerBounceSFX").GetComponent<AudioSource>();
        float screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        minX = screenLeft + basketHalfWidth;
        maxX = screenRight - basketHalfWidth;
    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            moveInput = 0;
            return;
        }

        if (inpMode == InputMode.Touch)
            TouchMove();
        else if (inpMode == InputMode.Accel)
            AccelMove();
    }

    void FixedUpdate()
    {
        Move();
    }


    void TouchMove()
    {
        if (Input.touchCount > 0)
        {
            if ((Input.mousePosition.x > (Screen.width * 0.6f)) && rb.position.x < maxX)
            {
                moveInput = 1f;
            }
            else if (Input.mousePosition.x <= (Screen.width * 0.4f) && rb.position.x > minX)
            {
                moveInput = -1;
            }
            else
            {
                moveInput = 0;
            }
        }
        else
        {
            moveInput = 0;
        }
    }

    void AccelMove()
    {
        if (Input.acceleration.x > 0.1f && rb.position.x < maxX)
        {
            moveInput = 1f;
        }
        else if (Input.acceleration.x < -0.1f && rb.position.x > minX)
        {
            moveInput = -1;
        }
        else
        {
            moveInput = 0;
            return;
        }
    }

    private void Move()
    {
        Vector3 direction = transform.right * moveInput;
        
        rb.AddForce(direction * movingSpeed, ForceMode2D.Force);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxXSpeed, maxXSpeed),
                                       Mathf.Clamp(rb.velocity.y, -maxYSpeed, maxYSpeed));

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        AudioManagerV2 audioManager = AudioManagerV2.Instance;
        
        if (col.CompareTag("Energy"))
        {
            GameManager.Instance.Score.Add(1);
            GameManagerScript.Instance.AddEnergy(1);
            if (audioManager != null) audioManager.PlayEnergy();
        }
        else if (col.CompareTag("Energy_5"))
        {
            GameManager.Instance.Score.Add(5);
            GameManagerScript.Instance.AddEnergy(5);
            if (audioManager != null) audioManager.PlayEnergy();
        }
        else if (col.CompareTag("Energy_15"))
        {             
            GameManager.Instance.Score.Add(15);
            GameManagerScript.Instance.AddEnergy(15);
            if (audioManager != null) audioManager.PlayEnergy();
        }
        else if (col.CompareTag("Bomb"))
        {
            if (audioManager != null) audioManager.PlayExplosion();
            GameManager.Instance.EndGame(false);
        }
    }

    void OnGUI()
    {
        if (inpMode == InputMode.Touch)
        {
            GUI.skin.label.fontSize = Screen.width / 20;
            GUILayout.Label("Touch");

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.position.x >= (Screen.width * 0.75f))
                {
                    GUI.DrawTexture(new Rect(touch.position.x, (touch.position.y * -1f) + (Screen.height), 200, 200), aTextureRight, ScaleMode.StretchToFill, true, 10.0F);
                    GUILayout.Label("Right");
                }

                if (touch.position.x <= (Screen.width * 0.25f))
                {
                    GUI.DrawTexture(new Rect(touch.position.x, (touch.position.y * -1f) + (Screen.height), 200, 200), aTextureLeft, ScaleMode.StretchToFill, true, 10.0F);
                    GUILayout.Label("Left");
                }
            }
        }

    }
}
