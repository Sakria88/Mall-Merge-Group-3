using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarUI : MonoBehaviour
{
    //The text that will display the star counter
    [SerializeField] private TMP_Text starText;

    private void OnEnable()
    {
       
            RefreshUI(); // update immediately
        
    }

    public void RefreshUI()
    {
        if (GameManagerScript.Instance != null)
        {
            starText.text = GameManagerScript.Instance.Stars.ToString();
        }
       
    }
    private void Update()
    {
        //Get the star amount from game manager and display
        starText.text = GameManagerScript.Instance.Stars.ToString();
    }
}
