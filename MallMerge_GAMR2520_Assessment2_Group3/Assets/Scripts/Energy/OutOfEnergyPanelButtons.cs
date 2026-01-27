using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfEnergyPanelButtons : MonoBehaviour
{
    // Change these to EXACT scene names from your project
    private string mainMenuScene = "MainMenu";
    private string miniGameScene = "MiniGame";

    // Called by Main Menu button
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    // Called by Mini Game button
    public void GoToMiniGame()
    {
        SceneManager.LoadScene(miniGameScene);
    }
}
