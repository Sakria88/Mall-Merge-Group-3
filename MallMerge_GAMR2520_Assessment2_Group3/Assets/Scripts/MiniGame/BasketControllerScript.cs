using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BasketControllerScript : MonoBehaviour
{
    public float movingSpeed = 5f;
    public float jumpForce = 10f;
    public float maxXSpeed = 5f;
    public float maxYSpeed = 15f;
    private int energyCounter = 0;

    float basketHalfWidth;
    float moveInput;
    float minX;
    float maxX;
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

        basketHalfWidth = GetComponent<Collider2D>().bounds.extents.x * 2;

        //playerBounceSFX = GameObject.Find("PlayerBounceSFX").GetComponent<AudioSource>();
        float screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        minX = screenLeft + basketHalfWidth;
        maxX = screenRight - basketHalfWidth;
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
    }

    //public void ChangeInputMode(Text btnText)
    //{
    //    if (inpMode == InputMode.Touch)
    //    {
    //        inpMode = InputMode.Accel;
    //    }
    //    else if (inpMode == InputMode.Accel)
    //    {
    //        inpMode = InputMode.Swipe;
    //    }
    //    else if (inpMode == InputMode.Swipe)
    //    {
    //        inpMode = InputMode.Touch;
    //    }

    //    btnText.text = inpMode.ToString();
    //}

    void TouchMove()
    {
        if (Input.touchCount > 0)
        {
            if ((Input.mousePosition.x > (Screen.width * 0.75f)) && rigBody.position.x < maxX)
            {
                moveInput = 1f;
            }
            else if (Input.mousePosition.x <= (Screen.width * 0.25f) && rigBody.position.x > minX)
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
        if (Input.acceleration.x > 0.1f && rigBody.position.x < maxX)
        {
            facingRight = true;
        }
        else if (Input.acceleration.x < -0.1f && rigBody.position.x > minX)
        {
            facingRight = false;
        }
        else
        {
            moveInput = 0;
            return;
        }
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Energy, Energy_5, Energy_15, Bomb
        if (collision.CompareTag("Energy"))
        {
            energyCounter++;
            Debug.Log("Energy Collected! Total Energy: " + energyCounter);
            Text text = GameObject.Find("EnergyButton_Text").GetComponent<Text>();
            text.text = "" + energyCounter;
        }
        else if (collision.CompareTag("Energy_5"))
        {
            energyCounter += 5;
            Debug.Log("Energy Collected! Total Energy: " + energyCounter);
            Text text = GameObject.Find("EnergyButton_Text").GetComponent<Text>();
            text.text = "" + energyCounter;
        }
        else if (collision.CompareTag("Energy_15"))
        {
            energyCounter += 15;
            Debug.Log("Energy Collected! Total Energy: " + energyCounter);
            Text text = GameObject.Find("EnergyButton_Text").GetComponent<Text>();
            text.text = "" + energyCounter;
        }
        else if (collision.CompareTag("Bomb"))
        {
            energyCounter -= 5;
            if (energyCounter < 0) energyCounter = 0;
            Debug.Log("Bomb Hit! Total Energy: " + energyCounter);
            Text text = GameObject.Find("EnergyButton_Text").GetComponent<Text>();
            text.text = "" + energyCounter;
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


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                GUI.DrawTexture(new Rect(touch.position.x, (touch.position.y * -1f) + (Screen.height), 200, 200), aTextureRight, ScaleMode.StretchToFill, true, 10.0F);
            }
        }
    }
}