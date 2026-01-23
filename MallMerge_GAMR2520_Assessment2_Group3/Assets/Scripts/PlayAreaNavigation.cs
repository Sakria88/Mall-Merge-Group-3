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

    public void GoToMenuSettings()
    {
        MenuManager.targetPanelName = "Settings";
        SceneManager.LoadScene("MallMergeMenus");
    }

    public void GoToMenuHelp()
    {
        MenuManager.targetPanelName = "Help";
        SceneManager.LoadScene("MallMergeMenus");
    }

    public void GoToMenuShop()
    {
        // tells MenuManager to open the Shop Catalogue panel
        MenuManager.targetPanelName = "Shop";
        SceneManager.LoadScene("MallMergeMenus");
    }

    public void GoToMainMenu()
    {
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