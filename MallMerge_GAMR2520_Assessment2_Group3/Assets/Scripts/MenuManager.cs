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

    // --- STATIC VARIABLES FOR PERSISTENCE & NAVIGATION ---
    public static string targetPanelName = ""; 
    private static MenuManager instance;

    private GameObject previousPanel; 
    private bool isMuted = false;
    private float preMuteVolume = 1f;

    // --- PERSISTENCE LOGIC (The "Secret Sauce") ---
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Keeps this object and its music playing across all scenes
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // If a second MenuManager spawns (when returning to menu), kill it immediately
            Destroy(gameObject); 
            return;
        }
    }

    void Start()
    {
        // 1. Force the instance to check for music immediately
        if (musicSource != null)
        {
            // Safety: If the volume is 0, set it to a default so we can hear it
            if (musicSource.volume <= 0) musicSource.volume = 0.5f;

            // Check if we already have a clip playing (from a previous scene)
            if (musicSource.clip == null || !musicSource.isPlaying)
            {
                // Load the last saved choice (Default to 0/Chilly if none exists)
                int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0); 
                
                // Explicitly play the correct clip
                AudioClip clipToPlay = (savedMusic == 0) ? chillyMusic : dreamMusic;
                PlaySong(clipToPlay);
            }
        }

        SetupUI(); 
    }

    // --- HELPER FOR UI INITIALIZATION ---
    private void SetupUI()
    {
        // 2. Initialize slider position
        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
        }

        // 3. Check if we redirected here from the Play Area
        if (!string.IsNullOrEmpty(targetPanelName))
        {
            if (targetPanelName == "Settings") SwitchPanel(settingsMenuPanel);
            else if (targetPanelName == "Help") SwitchPanel(helpMenuPanel);
            else if (targetPanelName == "Shop") SwitchPanel(shopCataloguePanel);

            targetPanelName = ""; 
        }
        else if (mainMenuPanel != null)
        {
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
        SceneManager.LoadScene("MainGamePlayArea");
    }

    public void OnSettingsButtonClicked() { PlayClickSound(); SwitchPanel(settingsMenuPanel); }
    public void OnShopButtonClicked() { PlayClickSound(); SwitchPanel(shopCataloguePanel); }
    public void OnHelpButtonClicked() { PlayClickSound(); SwitchPanel(helpMenuPanel); }

    // --- SETTINGS & NAVIGATION FUNCTIONS ---
    public void OnSettingsExitButtonClicked() { UniversalExit(); }
    public void OnMusicButtonClicked() { PlayClickSound(); SwitchPanel(musicMenuPanel); }
    public void OnMusicExitButtonClicked() { PlayClickSound(); SwitchPanel(settingsMenuPanel); }

    public void OnHelpExitButtonClicked()
    {
        PlayClickSound();
        if (previousPanel != null) SwitchPanel(previousPanel);
        else SceneManager.LoadScene("MainGamePlayArea");
    }

    public void BackToMainMenu()
    {
        PlayClickSound();
        SwitchPanel(mainMenuPanel);
        previousPanel = mainMenuPanel;
    }

    public void UniversalExit()
    {
        PlayClickSound();
        if (previousPanel != null && previousPanel != musicMenuPanel) SwitchPanel(previousPanel);
        else SwitchPanel(mainMenuPanel);
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

    // --- MUSIC SELECTION WITH SAVING ---
    public void OnOption1Clicked()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("SelectedMusic", 0);
        PlaySong(chillyMusic);
    }

    public void OnOption2Clicked()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("SelectedMusic", 1);
        PlaySong(dreamMusic);
    }

    private void PlaySong(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        
        // Change the clip if it's different OR if the source is currently stopped
        if (musicSource.clip != clip || !musicSource.isPlaying)
        {
            musicSource.clip = clip;
            musicSource.Play();
            musicSource.loop = true;
        }
    }

    // --- CORE LOGIC ---
    public void OnMiniGameButtonClicked()
    {
        PlayClickSound();
        SceneManager.LoadScene("MiniGameScene");
    }

    private void SwitchPanel(GameObject targetPanel)
    {
        if (targetPanel == null) return;

        if (mainMenuPanel != null && mainMenuPanel.activeSelf) previousPanel = mainMenuPanel;
        else if (shopCataloguePanel != null && shopCataloguePanel.activeSelf) previousPanel = shopCataloguePanel;
        else if (helpMenuPanel != null && helpMenuPanel.activeSelf) previousPanel = helpMenuPanel;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);
        if (shopCataloguePanel != null) shopCataloguePanel.SetActive(false);
        if (helpMenuPanel != null) helpMenuPanel.SetActive(false);
        if (musicMenuPanel != null) musicMenuPanel.SetActive(false);

        targetPanel.SetActive(true);
    }
}