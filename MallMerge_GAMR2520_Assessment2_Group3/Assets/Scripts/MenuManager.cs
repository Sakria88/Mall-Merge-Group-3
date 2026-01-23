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

    private GameObject previousPanel; 
    private bool isMuted = false;
    private float preMuteVolume = 1f;

    void Start()
    {
        if (musicSource != null && chillyMusic != null)
        {
            musicSource.clip = chillyMusic;
            musicSource.Play();
            musicSource.loop = true;
        }

        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
        }

        SwitchPanel(mainMenuPanel);
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

    // Assign this to your "Exit" button in Settings to force return to Main Menu
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
        // Record history ONLY if we are moving away from a 'root' panel
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