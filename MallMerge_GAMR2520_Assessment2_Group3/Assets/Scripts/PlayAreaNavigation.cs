using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAreaNavigation : MonoBehaviour
{
    public GameObject areaOne; // Drag Area One from your Canvas here

    void Start()
    {
        // This ensures Area One is active the moment the scene loads
        StartInAreaOne();
    }

    public void GoToMenuSettings()
    {
        MenuManager.targetPanelName = "Settings";
        SceneManager.LoadScene("MallMerge_Menus");
    }

    public void GoToMenuHelp()
    {
        MenuManager.targetPanelName = "Help";
        SceneManager.LoadScene("MallMerge_Menus");
    }

    public void GoToMenuShop()
    {
        // This tells MenuManager to open the Shop Catalogue panel
        MenuManager.targetPanelName = "Shop";
        SceneManager.LoadScene("MallMerge_Menus");
    }

    public void GoToMainMenu()
    {
        // Leaving targetPanelName empty will make MenuManager default to the Main Menu
        MenuManager.targetPanelName = "";
        SceneManager.LoadScene("MallMerge_Menus");
    }

    public void StartInAreaOne()
    {
        if (areaOne != null) 
        {
            areaOne.SetActive(true);
        }
    }
}