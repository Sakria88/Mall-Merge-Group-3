using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    public float speed = 10.0f;

    void Update()
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.acceleration.x;
        direction.z = Input.acceleration.y;

        transform.Translate((direction * speed) * Time.deltaTime);
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 20;
        GUILayout.Label("Accelerometer");
        GUILayout.Label("Input.acceleration: " + Input.acceleration);
    }
}
