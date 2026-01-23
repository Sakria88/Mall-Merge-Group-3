using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAreaNavigation : MonoBehaviour
{
    public GameObject areaOne; // Drag Area One from your Canvas here

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

    public void StartInAreaOne()
    {
        // This ensures Area One is active when the play scene starts
        if (areaOne != null) areaOne.SetActive(true);
    }
}