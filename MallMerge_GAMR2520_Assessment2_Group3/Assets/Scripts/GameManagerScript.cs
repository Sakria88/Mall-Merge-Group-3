using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    //Star currency
    private const string Stars_Key = "Player_Stars";


    //Energy
    private const string Energy_Key = "Player_Energy";
    public int maxEnergy = 60;

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

    }

    //Loading and saving the star and energy information

    private void ItemData()
    {
        int defaultEnergy = maxEnergy;
        int stars = PlayerPrefs.GetInt(Stars_Key, 0);
        int currentEnergy = PlayerPrefs.GetInt(Energy_Key, 0);

        PlayerPrefs.SetInt(Stars_Key, stars);
        PlayerPrefs.SetInt(Energy_Key, currentEnergy);
        PlayerPrefs.Save();
    }

    private void SaveStarData(int value)
    {
        PlayerPrefs.SetInt(Stars_Key, value);
        PlayerPrefs.Save();
    }

    private void SaveEnergyData(int value)
    {
        PlayerPrefs.SetInt(Energy_Key, value);
        PlayerPrefs.Save();
    }
    // --- STAR LOGIC ---

    public int Stars => PlayerPrefs.GetInt(Stars_Key, 0);
    public void AddStars(int amount)
    {
        int updatesStars = Stars + amount;
        SaveStarData(updatesStars);
        Debug.Log("Stars added! Current balance: " + updatesStars);
    }

    public bool SpendStars(int amount)
    {
        if (Stars < amount)
            return false;

        SaveStarData(Stars - amount);
        return true;
    }

    // --- ENERGY LOGIC ---
    public int currentEnergy => PlayerPrefs.GetInt(Energy_Key, maxEnergy);
    public bool TrySpendEnergy(int amount = 1)
    {
        if (currentEnergy < amount)
            return false;

        SaveEnergyData(currentEnergy - amount);
        
        return true;
    }

    public void AddEnergy(int amount)
    {
       int updatedEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        SaveEnergyData(updatedEnergy);
    }

    public bool BuyEnergy(int starCost, int gainEnergy)
    {
        

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
