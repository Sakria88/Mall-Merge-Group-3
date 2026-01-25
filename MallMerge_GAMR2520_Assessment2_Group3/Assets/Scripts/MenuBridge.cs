using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBridge : MonoBehaviour
{
    // A helper to find the active MenuManager instance
    private MenuManager GetManager()
    {
        return FindObjectOfType<MenuManager>();
    }

    public void OpenPlay() => GetManager().OnPlayButtonClicked();
    public void OpenSettings() => GetManager().OnSettingsButtonClicked();
    public void OpenShop() => GetManager().OnShopButtonClicked();
    public void OpenHelp() => GetManager().OnHelpButtonClicked();
    public void BackToMain() => GetManager().BackToMainMenu();
}