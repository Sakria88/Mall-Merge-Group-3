using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsMenuPanel;
    public GameObject shopCataloguePanel;
    public GameObject helpMenuPanel;
    public GameObject musicMenuPanel;

    private GameObject previousPanel; // Tracks where we came from for the 'Exit' button

    void Start()
    {
        // This ensures that even if you left the Settings panel open in the editor,
        // the game starts fresh on the Main Menu.
        SwitchPanel(mainMenuPanel);
    }

    // --- MAIN MENU FUNCTIONS ---

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Main Game Play Area");
    }

    public void OnSettingsButtonClicked()
    {
        SwitchPanel(settingsMenuPanel);
    }

    public void OnShopButtonClicked()
    {
        SwitchPanel(shopCataloguePanel);
    }

    public void OnHelpButtonClicked()
    {
        SwitchPanel(helpMenuPanel);
    }

    // --- SETTINGS FUNCTIONS ---

    public void OnSettingsExitButtonClicked()
    {
        if (previousPanel != null)
        {
            SwitchPanel(previousPanel);
        }
        else
        {
            SwitchPanel(mainMenuPanel);
        }
    }

    public void OnSettingsBackToMainClicked()
    {
        SwitchPanel(mainMenuPanel);
    }

    public void OnMusicButtonClicked()
    {
        SwitchPanel(musicMenuPanel);
    }

    // --- OTHER FUNCTIONS ---

    public void OnMiniGameButtonClicked()
    {
        SceneManager.LoadScene("mini game scene");
    }

    // --- CORE LOGIC ---

    private void SwitchPanel(GameObject targetPanel)
    {
        // 1. Record the current active panel as 'previous' before we hide it
        // We only track history if we aren't already in a sub-menu to avoid loops
        if (mainMenuPanel.activeSelf) previousPanel = mainMenuPanel;
        else if (shopCataloguePanel.activeSelf) previousPanel = shopCataloguePanel;
        else if (helpMenuPanel.activeSelf) previousPanel = helpMenuPanel;

        // 2. Turn everything off
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        shopCataloguePanel.SetActive(false);
        helpMenuPanel.SetActive(false);
        musicMenuPanel.SetActive(false);

        // 3. Turn the requested panel on
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
    }
}