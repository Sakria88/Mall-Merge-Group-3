using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    public float targetTime = 60.0f;

    void Start()
    {

        Debug.Log("Time left: " + (int)(targetTime));
        Text text = GameObject.Find("Timer_Text").GetComponent<Text>();
        text.text = "" + targetTime;
    }

    void Update()
    {
        targetTime -= Time.deltaTime;

        Debug.Log("Time left: " + (int)targetTime);
        Text text = GameObject.Find("Timer_Text").GetComponent<Text>();
        text.text = "" + targetTime;

        if (targetTime < 10.0f)
        {
            text.color = Color.red;
        }

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }

    }

    void timerEnded()
    {
        Text text = GameObject.Find("Timer_Text").GetComponent<Text>();
        text.text = "Time's Up!";
        text.color = Color.black;

        //end BasketControllerScript
    }
}