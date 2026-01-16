using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeGesture : MonoBehaviour
{
    Rigidbody rigBody;
    Vector2 fingerTouchDown;
    Vector2 fingerTouchUp;
    float gestureBuffer = 20f;
    float torqueValueNormalised;
    float torqueValue;
    float minTorque = 1;
    float maxTorque = 15;
    string swipes;
    List<string> swipesList = new List<string>();

    private void Start()
    {
        rigBody = GetComponent<Rigidbody>();
        rigBody.maxAngularVelocity = 20f;
    }

    void Update()
    {

        Debug.Log(Screen.dpi);
        Debug.Log(Mathf.InverseLerp(250, 500, Screen.dpi));
        Debug.Log(Mathf.Lerp(20, 50, Mathf.InverseLerp(250, 500, Screen.dpi)));


        if (swipesList.Count == 9)
        {
            swipesList.RemoveAt(0);
        }

        if(Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                fingerTouchDown = Input.touches[0].position;
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                fingerTouchUp = Input.touches[0].position;
                CheckSwipe();
            }
        }    
    }

    void CheckSwipe()
    {
        string currentSwipes = "";

        if (Mathf.Abs(fingerTouchDown.x - fingerTouchUp.x) > gestureBuffer)  
        {
            torqueValueNormalised = Mathf.InverseLerp(gestureBuffer, Screen.width, Mathf.Abs(fingerTouchDown.x - fingerTouchUp.x));
            torqueValue = Mathf.Lerp(minTorque, maxTorque, torqueValueNormalised);

            if (fingerTouchDown.x - fingerTouchUp.x < 0)
            {
                currentSwipes += OnSwipeRight(torqueValue);
            }
            else if (fingerTouchDown.x - fingerTouchUp.x > 0)
            {
                currentSwipes += OnSwipeLeft(torqueValue);
            }
        }

        if (Mathf.Abs(fingerTouchDown.y - fingerTouchUp.y) > gestureBuffer)
        {
            torqueValueNormalised = Mathf.InverseLerp(gestureBuffer, Screen.height, Mathf.Abs(fingerTouchDown.y - fingerTouchUp.y));
            torqueValue = Mathf.Lerp(minTorque, maxTorque, torqueValueNormalised);

            if (fingerTouchDown.y - fingerTouchUp.y < 0)
            {
                currentSwipes += OnSwipeUp(torqueValue);
            }
            else if (fingerTouchDown.y - fingerTouchUp.y > 0)
            {
                currentSwipes += OnSwipeDown(torqueValue);
            }
        }

        swipesList.Add(currentSwipes + "\n");
        swipes = "";
        for (int i = 0; i < swipesList.Count; i++)
        {
            swipes += swipesList[i];
        }
    }

    string OnSwipeLeft(float torqueValue)
    {
        rigBody.AddTorque(new Vector3(0, torqueValue, 0));
        return ("←: " + torqueValue.ToString("0.0") + "; ");
    }

    string OnSwipeRight(float torqueValue)
    {
        rigBody.AddTorque(new Vector3(0, -torqueValue, 0));
        return ("→: " + torqueValue.ToString("0.0") + "; ");
    }

    string OnSwipeUp(float torqueValue)
    {
        rigBody.AddTorque(new Vector3(torqueValue, 0, 0));
        return ("↑: " + torqueValue.ToString("0.0") + "; ");
          
    }

    string OnSwipeDown(float torqueValue)
    {
        rigBody.AddTorque(new Vector3(-torqueValue, 0, 0));
        return ("↓: " + torqueValue.ToString("0.0") + "; ");
    }


    void OnGUI()
    {
        GUI.skin.box.fontSize = (int)Mathf.Lerp(20, 100, Mathf.InverseLerp(250, 500, Screen.dpi));
        GUI.skin.box.alignment = TextAnchor.UpperLeft;
        GUI.backgroundColor = Color.red;
        GUI.Box(new Rect(10, Screen.height * 0.5f, Screen.width - 20, (Screen.height * 0.5f - 10)), "Swipe Gestures: \n" + swipes);
    }
}
