using UnityEngine;
using TMPro;

/// <summary>
/// Simple, stable energy system.
/// - Starts with maxEnergy
/// - Spend(1) on merges
/// - When energy hits 0: shows OutOfEnergyPanel
/// - Provides CanPlay() so other scripts can block input/spawn
/// </summary>
public class EnergyManager : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private int maxEnergy = 20;
    [SerializeField] private int currentEnergy = 20;

    [Header("UI")]
    [SerializeField] private TMP_Text energyCounterText;    // your Energy Counter TMP text
    [SerializeField] private GameObject outOfEnergyPanel;   // your popup panel GO

    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;

    private void Awake()
    {
        // Ensure panel starts hidden (avoid flicker / repeated enabling)
        if (outOfEnergyPanel != null)
            outOfEnergyPanel.SetActive(false);

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        RefreshUI();
    }

    /// <summary>
    /// True if player still has energy and can play.
    /// </summary>
    public bool CanPlay()
    {
        return currentEnergy > 0;
    }

    /// <summary>
    /// Try to spend energy. Returns true if spent.
    /// If not enough, shows OutOfEnergyPanel once.
    /// </summary>
    public bool TrySpend(int amount = 1)
    {
        if (amount <= 0) return true;

        if (currentEnergy < amount)
        {
            ShowOutOfEnergy();
            return false;
        }

        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        RefreshUI();

        if (currentEnergy == 0)
            ShowOutOfEnergy();

        return true;
    }

    /// <summary>
    /// Call this from minigame reward etc.
    /// </summary>
    public void AddEnergy(int amount)
    {
        if (amount <= 0) return;

        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        RefreshUI();

        // If you got energy back, hide panel.
        if (currentEnergy > 0 && outOfEnergyPanel != null)
            outOfEnergyPanel.SetActive(false);
    }

    private void RefreshUI()
    {
        if (energyCounterText != null)
            energyCounterText.text = currentEnergy.ToString();
    }

    private void ShowOutOfEnergy()
    {
        if (outOfEnergyPanel != null && !outOfEnergyPanel.activeSelf)
            outOfEnergyPanel.SetActive(true);
    }
}
