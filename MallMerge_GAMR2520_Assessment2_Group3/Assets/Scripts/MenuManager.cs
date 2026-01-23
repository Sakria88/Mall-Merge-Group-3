using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsMenuPanel;
    public GameObject shopCataloguePanel;
    public GameObject helpMenuPanel;
    public GameObject musicMenuPanel; // New Panel

    private GameObject previousPanel; // Tracks the last active panel

    // --- SETTINGS NAV FUNCTIONS ---

    // 1. Exit Button: Go back to whatever page it was on before Settings
    public void OnSettingsExitButtonClicked()
    {
        if (previousPanel != null)
        {
            SwitchPanel(previousPanel);
        }
        else
        {
            // Default to Main Menu if no history exists
            SwitchPanel(mainMenuPanel);
        }
    }

    // 2. Back Button: Always go to Main Menu
    public void OnSettingsBackToMainClicked()
    {
        SwitchPanel(mainMenuPanel);
    }

    // 3. Music Button: Go to Music Menu Panel
    public void OnMusicButtonClicked()
    {
        SwitchPanel(musicMenuPanel);
    }

    // Updated SwitchPanel to track history
    private void SwitchPanel(GameObject targetPanel)
    {
        // Record the current panel as 'previous' before switching, 
        // unless we are already in settings/music to avoid loops.
        if (mainMenuPanel.activeSelf) previousPanel = mainMenuPanel;
        else if (shopCataloguePanel.activeSelf) previousPanel = shopCataloguePanel;
        else if (helpMenuPanel.activeSelf) previousPanel = helpMenuPanel;

        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        shopCataloguePanel.SetActive(false);
        helpMenuPanel.SetActive(false);
        musicMenuPanel.SetActive(false);

        targetPanel.SetActive(true);
    }
}