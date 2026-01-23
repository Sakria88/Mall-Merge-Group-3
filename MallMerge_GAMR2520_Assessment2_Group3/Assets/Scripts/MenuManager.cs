using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsMenuPanel;
    public GameObject shopCataloguePanel;
    public GameObject helpMenuPanel;

    // --- BUTTON FUNCTIONS ---

    // 1. Play Button: Go to last saved area in "Main Game Play Area"
    public void OnPlayButtonClicked()
    {
        // SceneManager.LoadScene will load the specified scene. 
        // Logic for "last saved area" would typically be handled 
        // within that scene's initialization.
        SceneManager.LoadScene("Main Game Play Area");
    }

    // 2. Settings Button: Open Settings Menu Panel
    public void OnSettingsButtonClicked()
    {
        SwitchPanel(settingsMenuPanel);
    }

    // 3. Shop Button: Open Shop Catalogue Panel
    public void OnShopButtonClicked()
    {
        SwitchPanel(shopCataloguePanel);
    }

    // 4. Help Button: Open Help Menu Panel
    public void OnHelpButtonClicked()
    {
        SwitchPanel(helpMenuPanel);
    }

    // 5. Mini Game Button (Inside Shop/Mini Game Panel)
    public void OnMiniGameButtonClicked()
    {
        SceneManager.LoadScene("mini game scene");
    }

    // Helper method to toggle visibility
    private void SwitchPanel(GameObject targetPanel)
    {
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        shopCataloguePanel.SetActive(false);
        helpMenuPanel.SetActive(false);

        targetPanel.SetActive(true);
    }
}