using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PinchAndZoom : MonoBehaviour
{
    Camera mainCamera;
    public float perspectiveZoomSpeed = 0.1f;  
    public float orthoZoomSpeed = 0.1f;

    public Texture aTextureLeft;
    public Texture aTextureRight;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 0.1f);
            }
            else
            {
                mainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 0.1f, 179.9f);
            }
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 20;
        GUILayout.Label("Pinch And Zoom");
        GUILayout.Label("Input.count: " + Input.touchCount);

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.position.x >= (Screen.width / 2))
            {
                GUI.DrawTexture(new Rect(touch.position.x, (touch.position.y * -1f) + (Screen.height), 500, 500), aTextureRight, ScaleMode.StretchToFill, true, 10.0F);
            }
            else
            {
                GUI.DrawTexture(new Rect(touch.position.x, (touch.position.y * -1f) + (Screen.height), 500, 500), aTextureLeft, ScaleMode.StretchToFill, true, 10.0F);
            }
        }
    }
}
