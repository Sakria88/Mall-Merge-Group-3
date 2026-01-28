using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

  //Star currency
    public int stars = 0;

    //Energy
    public int maxEnergy = 20;
    public int currentEnergy = 20;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Audio Setup
        float savedVolume = PlayerPrefs.GetFloat("AudioLevel", 1);
        AudioListener.volume = savedVolume;

        // Safety check: only update slider if one exists in the current scene
        Slider volumeSlider = FindObjectOfType<Slider>();
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        // Initialize Energy
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    // --- STAR LOGIC ---

    public void AddStars(int amount)
    {
        stars += amount;
        Debug.Log("Stars added! Current balance: " + stars);
    }

    public bool SpendStars(int amount)
    {
        if (stars < amount)
            return false;

        stars -= amount;
        return true;
    }

    // --- ENERGY LOGIC ---

    public bool TrySpendEnergy(int amount = 1)
    {
        if (currentEnergy < amount)
            return false;

        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        return true;
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    public bool BuyEnergy(int starCost, int gainEnergy)
    {
        //Debug.Log("Trying to buy energy...");
        //Debug.Log("Stars before purchase: " + stars);
        //Debug.Log("Cost: " + starCost);

        if (!SpendStars(starCost))
            return false;
        Debug.Log("NOT ENOUGHS");

        AddEnergy(gainEnergy);
        Debug.Log("Purchase Success");
        return true;
    }

    public bool CanPlay()
    {
        return currentEnergy > 0;
    }

    // --- AUDIO LOGIC ---

    public void AudioValueChange(Slider slider)
    {
        PlayerPrefs.SetFloat("AudioLevel", slider.value);
        AudioListener.volume = slider.value;
    }
}
