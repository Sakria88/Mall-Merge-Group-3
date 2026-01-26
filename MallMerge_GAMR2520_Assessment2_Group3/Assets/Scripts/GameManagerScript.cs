using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("AudioLevel", 1);
        FindObjectOfType<Slider>().value = PlayerPrefs.GetFloat("AudioLevel", 1);
    }

    public void AudioValueChange(Slider slider)
    {
        PlayerPrefs.SetFloat("AudioLevel", slider.value);
        AudioListener.volume = slider.value;
    }
}
