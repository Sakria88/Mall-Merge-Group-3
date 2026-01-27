using UnityEngine;
using TMPro;

/// <summary>
/// 
/// 
/// </summary>
public class EnergyManager : MonoBehaviour
{
    // The text that will display the energy number
    [SerializeField] private TMP_Text energyCounterText;

    //A popup that will display the energy at 0
    [SerializeField] private GameObject OutOfEnergyPanel;

    private void Awake()
    {
        //The out of energy panel will be hidden when the game starts
        if(OutOfEnergyPanel != null)
            OutOfEnergyPanel.SetActive(false);

    }
    private void Update()
    {
        //Update the energy number each frame
        RefreshUI();

        //If the energy is 0 show the out of energy panel
        if (GameManagerScript.Instance.currentEnergy <= 0)
            ShowOutOfEnergy();
        else
            OutOfEnergyPanel.SetActive(false);
    }

    //Updating the text on the screen

    private void RefreshUI()
    {
        energyCounterText.text= GameManagerScript.Instance.currentEnergy.ToString();

    }

    //Show the out of energy panel
    private void ShowOutOfEnergy()
    {
        if (!OutOfEnergyPanel.activeSelf)
            OutOfEnergyPanel.SetActive(true);
    }
}
