using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    public float movingSpeed = 5f;
    public float jumpForce = 10f;
    public float maxXSpeed = 5f;
    public float maxYSpeed = 15f;
    float moveInput;
    bool isGrounded;
    Rigidbody2D rigBody;

    public enum InputMode { Touch, Accel, Swipe }
    public InputMode inpMode = InputMode.Touch;

    Vector2 fingerDown;
    Vector2 fingerUp;

    public Texture aTextureLeft;
    public Texture aTextureRight;
    string swipe = "";

    Camera cam;
    bool facingRight = false;

    AudioSource playerBounceSFX;

    void Start()
    {
        rigBody = GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();

        playerBounceSFX = GameObject.Find("PlayerBounceSFX").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (inpMode == InputMode.Touch)
        {
            TouchMove();
        }
        else if (inpMode == InputMode.Accel)
        {
            AccelMove();
        }
        else if (inpMode == InputMode.Swipe)
        {
            SwipeMove();
        }
        else
        {
            moveInput = 0;
        }

        Move();

        if (isGrounded)
        {
            rigBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            playerBounceSFX.Play();
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        ScreenSwap();
    }

    public void ChangeInputMode(Text btnText)
    {
        if (inpMode == InputMode.Touch)
        {
            inpMode = InputMode.Accel;
        }
        else if (inpMode == InputMode.Accel)
        {
            inpMode = InputMode.Swipe;
        }
        else if (inpMode == InputMode.Swipe)
        {
            inpMode = InputMode.Touch;
        }

        btnText.text = inpMode.ToString();
    }

    void TouchMove()
    {
        if (Input.touchCount > 0)
        {
            if (Input.mousePosition.x > (Screen.width * 0.75f))
            {
                moveInput = 1f;
            }
            else if (Input.mousePosition.x <= (Screen.width * 0.25f))
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
        moveInput = Input.acceleration.x;
    }

    void SwipeMove()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                fingerDown = Input.touches[0].position;
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                fingerUp = Input.touches[0].position;
                CheckSwipe();
            }
        }
        else
        {
            moveInput = 0;
        }
    }

    void ScreenSwap()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        float screenRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z)).x;
        float screenLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.transform.position.z)).x;

        if (screenPos.x <= 0 && rigBody.velocity.x < 0)
        {
            transform.position = new Vector2(screenLeft, transform.position.y);
        }
        else if (screenPos.x >= Screen.width && rigBody.velocity.x > 0)
        {
            transform.position = new Vector2(screenRight, transform.position.y);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 wrapAround = transform.localScale;
        wrapAround.x *= -1;
        transform.localScale = wrapAround;
    }

    void CheckSwipe()
    {
        if (fingerDown.x - fingerUp.x < 0)
        {
            OnSwipeRight();
        }
        else if (fingerDown.x - fingerUp.x > 0)
        {
            OnSwipeLeft();
        }
    }

    void OnSwipeLeft()
    {
        moveInput = -1;
        swipe = "Left";
    }
    void OnSwipeRight()
    {
        moveInput = 1; 
        swipe = "Right";
    }

    private void Move()
    {
        Vector3 direction = transform.right * moveInput;
        if (inpMode == InputMode.Swipe)
        {
            rigBody.AddForce(direction * movingSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rigBody.AddForce(direction * movingSpeed, ForceMode2D.Force);
        }
        rigBody.velocity = new Vector2(Mathf.Clamp(rigBody.velocity.x, -maxXSpeed, maxXSpeed),
                                       Mathf.Clamp(rigBody.velocity.y, -maxYSpeed, maxYSpeed));
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !isGrounded)
        {
            isGrounded = true;
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
        else if (inpMode == InputMode.Accel)
        {
            GUI.skin.label.fontSize = Screen.width / 20;
            GUILayout.Label("Accelerometer");
            GUILayout.Label("Input.acceleration.x: " + Input.acceleration.x);

            if (Input.acceleration.x > 0.1f)
            {
                GUILayout.Label("Right");
            }
            else if (Input.acceleration.x < -0.1f)
            {
                GUILayout.Label("Left");
            }
        }
        else if (inpMode == InputMode.Swipe)
        {
            GUI.skin.label.fontSize = Screen.width / 20;
            GUILayout.Label("Swipe");
            GUILayout.Label("Gesture: " + swipe);


            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                GUI.DrawTexture(new Rect(touch.position.x, (touch.position.y * -1f) + (Screen.height), 200, 200), aTextureRight, ScaleMode.StretchToFill, true, 10.0F);
            }
        }
    }

}
