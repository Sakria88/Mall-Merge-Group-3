using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAreaNavigation : MonoBehaviour
{
    public GameObject areaOne; 

    void Start()
    {
        // ensures Area One is active the moment the scene loads
        StartInAreaOne();
    }

    // --- UNIVERSITY ASSIGNMENT METHOD ---
    // This allows you to load any scene by typing its name in the Inspector
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // --- EXISTING NAVIGATION METHODS ---
    public void GoToMenuSettings()
    {
        Debug.Log("Navigation: Setting target to Settings");
        MenuManager.targetPanelName = "Settings_Panel"; 
        SceneManager.LoadScene("MallMergeMenus");
    }

    public void GoToMenuHelp()
    {
        Debug.Log("Navigation: Setting target to Help");
        MenuManager.targetPanelName = "Help_Panel";
        SceneManager.LoadScene("MallMergeMenus");
    }

    public void GoToMenuShop()
    {
        Debug.Log("Navigation: Setting target to Shop");
        // tells MenuManager to open the Shop Catalogue panel
        MenuManager.targetPanelName = "ShopCatalogue_Panel";
       SceneManager.LoadScene("MallMergeMenus");
    }

    public void GoToMainMenu()
    {
        Debug.Log("Navigation: Setting target to MainMenu (Empty)");
        // Leaving targetPanelName empty will make MenuManager default to the Main Menu
        MenuManager.targetPanelName = "";
       SceneManager.LoadScene("MallMergeMenus");
    }

    public void StartInAreaOne()
    {
        if (areaOne != null) 
        {
            areaOne.SetActive(true);
        }
    }
}