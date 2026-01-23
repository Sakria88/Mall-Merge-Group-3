using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsMenuPanel;
    public GameObject shopCataloguePanel;
    public GameObject helpMenuPanel;
    public GameObject musicMenuPanel;

    [Header("Audio Settings")]
    public AudioSource musicSource;
    public AudioClip chillyMusic; 
    public AudioClip dreamMusic;  

    [Header("Sound Effects")]
    public AudioClip buttonClickSFX;

    [Header("Volume UI")]
    public Slider volumeSlider;
    public Image muteButtonImage;
    public Sprite volumeSprite;
    public Sprite muteSprite;

    // --- NEW STATIC VARIABLE FOR CROSS-SCENE NAVIGATION ---
    public static string targetPanelName = ""; 

    private GameObject previousPanel; 
    private bool isMuted = false;
    private float preMuteVolume = 1f;

    void Start()
    {
        // 1. Initialize Music
        if (musicSource != null && chillyMusic != null)
        {
            musicSource.clip = chillyMusic;
            musicSource.Play();
            musicSource.loop = true;
        }

        // 2. Initialize slider position
        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
        }

        // 3. NEW: Check if we redirected here from the Play Area
        if (!string.IsNullOrEmpty(targetPanelName))
        {
            if (targetPanelName == "Settings") SwitchPanel(settingsMenuPanel);
            else if (targetPanelName == "Help") SwitchPanel(helpMenuPanel);
            else if (targetPanelName == "Shop") SwitchPanel(shopCataloguePanel);
            
            // Reset it so the menu behaves normally next time
            targetPanelName = ""; 
        }
        else
        {
            // Default start
            SwitchPanel(mainMenuPanel);
        }
    }

    // --- SOUND EFFECTS ---

    public void PlayClickSound()
    {
        if (musicSource != null && buttonClickSFX != null)
        {
            musicSource.PlayOneShot(buttonClickSFX);
        }
    }

    // --- MAIN MENU FUNCTIONS ---

    public void OnPlayButtonClicked()
    {
        PlayClickSound();
        
        // Ensure "Main Game Play Area" matches your Scene name in Build Settings exactly
        SceneManager.LoadScene("Main Game Play Area");
    }

    public void OnSettingsButtonClicked()
    {
        PlayClickSound();
        SwitchPanel(settingsMenuPanel);
    }

    public void OnShopButtonClicked()
    {
        PlayClickSound();
        SwitchPanel(shopCataloguePanel);
    }

    public void OnHelpButtonClicked()
    {
        PlayClickSound();
        SwitchPanel(helpMenuPanel);
    }

    // --- SETTINGS & NAVIGATION FUNCTIONS ---

    public void OnSettingsExitButtonClicked()
    {
        UniversalExit();
    }

    public void OnMusicButtonClicked()
    {
        PlayClickSound();
        SwitchPanel(musicMenuPanel);
    }

    public void OnMusicExitButtonClicked()
    {
        PlayClickSound();
        SwitchPanel(settingsMenuPanel);
    }

    public void OnHelpExitButtonClicked()
    {
        PlayClickSound();
        if (previousPanel != null)
        {
            SwitchPanel(previousPanel);
        }
        else
        {
            SceneManager.LoadScene("Main Game Play Area");
        }
    }

    // --- SPECIFIC EXIT LOGIC ---

    public void BackToMainMenu()
    {
        PlayClickSound();
        SwitchPanel(mainMenuPanel);
        previousPanel = mainMenuPanel;
    }

    public void UniversalExit()
    {
        PlayClickSound();
        if (previousPanel != null && previousPanel != musicMenuPanel)
        {
            SwitchPanel(previousPanel);
        }
        else
        {
            SwitchPanel(mainMenuPanel);
        }
    }

    // --- AUDIO CONTROLS ---

    public void SetVolume(float volume)
    {
        if (musicSource == null) return;
        musicSource.volume = volume;

        if (volume > 0 && isMuted)
        {
            isMuted = false;
            if (muteButtonImage != null) muteButtonImage.sprite = volumeSprite;
        }
        else if (volume <= 0 && !isMuted)
        {
            isMuted = true;
            if (muteButtonImage != null) muteButtonImage.sprite = muteSprite;
        }
    }

    public void ToggleMute()
    {
        if (musicSource == null) return;
        PlayClickSound();
        isMuted = !isMuted;

        if (muteButtonImage != null)
            muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;

        if (isMuted)
        {
            preMuteVolume = musicSource.volume;
            musicSource.volume = 0f;
            if (volumeSlider != null) volumeSlider.value = 0f;
        }
        else
        {
            float restoreVol = preMuteVolume > 0.1f ? preMuteVolume : 0.5f;
            musicSource.volume = restoreVol;
            if (volumeSlider != null) volumeSlider.value = restoreVol;
        }
    }

    // --- MUSIC SELECTION ---

    public void OnOption1Clicked()
    {
        PlayClickSound();
        PlaySong(chillyMusic);
    }

    public void OnOption2Clicked()
    {
        PlayClickSound();
        PlaySong(dreamMusic);
    }

    private void PlaySong(AudioClip clip)
    {
        if (musicSource == null || clip == null || musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    // --- CORE LOGIC ---

    public void OnMiniGameButtonClicked()
    {
        PlayClickSound();
        SceneManager.LoadScene("mini game scene");
    }

    private void SwitchPanel(GameObject targetPanel)
    {
        if (mainMenuPanel.activeSelf) previousPanel = mainMenuPanel;
        else if (shopCataloguePanel.activeSelf) previousPanel = shopCataloguePanel;
        else if (helpMenuPanel.activeSelf) previousPanel = helpMenuPanel;

        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        shopCataloguePanel.SetActive(false);
        helpMenuPanel.SetActive(false);
        musicMenuPanel.SetActive(false);

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
    }
}