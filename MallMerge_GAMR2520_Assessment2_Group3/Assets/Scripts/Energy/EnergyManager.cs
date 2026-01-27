using UnityEngine;
using TMPro;

/// <summary>
/// Controls player energy.
/// Energy is spent when a merge happens.
/// Shows a popup when energy reaches 0.
/// </summary>
///


public class EnergyManager : MonoBehaviour
{
    



    [Header("Energy Settings")]
    public int maxEnergy = 20;
    public int currentEnergy = 20;

    [Header("UI")]
    public TMP_Text energyText;              // drag your energy number text here
    public GameObject outOfEnergyPanel;      // drag your popup panel here

    private void Start()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        RefreshUI();
    }

    public bool HasEnergy(int amount = 1)
    {
        return currentEnergy >= amount;
    }

    public bool TrySpendEnergy(int amount = 1)
    {
        if (!HasEnergy(amount))
        {
            ShowOutOfEnergyPanel();
            return false;
        }

        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        RefreshUI();

        if (currentEnergy == 0)
        {
            ShowOutOfEnergyPanel();
        }

        return true;
    }

    private void RefreshUI()
    {
        if (energyText != null)
            energyText.text = currentEnergy.ToString();
    }

    private void ShowOutOfEnergyPanel()
    {
        if (outOfEnergyPanel != null)
            outOfEnergyPanel.SetActive(true);
    }
}
