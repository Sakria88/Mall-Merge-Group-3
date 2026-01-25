using UnityEngine;

public class MenuBridge : MonoBehaviour
{
    // Helpers to find the active scripts in the current scene
    private MenuManager GetManager() => FindObjectOfType<MenuManager>();
    private PlayAreaNavigation GetNav() => FindObjectOfType<PlayAreaNavigation>();

    // --- FOR THE MAIN MENU SCENE ---
    // Use these for buttons physically located in the "MallMergeMenus" scene
    public void OpenPlay() => GetManager().OnPlayButtonClicked();
    public void OpenSettings() => GetManager().OnSettingsButtonClicked();
    public void OpenShop() => GetManager().OnShopButtonClicked();
    public void OpenHelp() => GetManager().OnHelpButtonClicked();
    public void BackToMain() => GetManager().BackToMainMenu();

    // --- FOR THE GAMEPLAY SCENE (MainGamePlayArea) ---
    // Use these for buttons physically located in the "MainGamePlayArea" scene
    // These use your PlayAreaNavigation logic to switch scenes and set the target panel
    public void GameToMenuSettings() => GetNav().GoToMenuSettings();
    public void GameToMenuShop() => GetNav().GoToMenuShop();
    public void GameToMenuHelp() => GetNav().GoToMenuHelp();
    public void GameToMainMenu() => GetNav().GoToMainMenu();
}