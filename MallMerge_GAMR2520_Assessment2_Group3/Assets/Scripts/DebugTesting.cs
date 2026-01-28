using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTesting : MonoBehaviour
{
    void Start()
    {
        // Set stars to 50 for testing
        PlayerPrefs.SetInt("Player_Stars", 50);
        PlayerPrefs.SetInt("Player_Energy", 30);
        PlayerPrefs.Save();

        // Update UI
        foreach (var ui in FindObjectsOfType<StarUI>())
            ui.RefreshUI();

        foreach (var ui in FindObjectsOfType<EnergyManager>())
            ui.RefreshUI();

        Debug.Log("Debug values applied!");
    }
}
