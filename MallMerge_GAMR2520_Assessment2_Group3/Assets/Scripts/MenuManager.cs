using UnityEngine;
using UnityEngine.UI; // Needed for Slider and Image
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
    public AudioClip chillyMusic; // Option 1
    public AudioClip dreamMusic;  // Option 2

    [Header("Volume UI")]
    public Slider volumeSlider;
    public Image muteButtonImage;
    public Sprite volumeSprite;
    public Sprite muteSprite;

    private GameObject previousPanel; // Tracks where we came from for the 'Exit' button
    private bool isMuted = false;
    private float preMuteVolume = 1f;

    void Start()
    {
        // 1. Initialize Music (Start with Chilly.wav)
        if (musicSource != null && chillyMusic != null)
        {
            musicSource.clip = chillyMusic;
            musicSource.Play();
            musicSource.loop = true;
        }

        // 2. Initialize slider position to current volume
        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
        }

        // 3. Initialize UI (Start on Main Menu)
        SwitchPanel(mainMenuPanel);
    }

    // --- MAIN MENU FUNCTIONS ---

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Main Game Play Area");
    }

    public void OnSettingsButtonClicked()
    {
        SwitchPanel(settingsMenuPanel);
    }

    public void OnShopButtonClicked()
    {
        SwitchPanel(shopCataloguePanel);
    }

    public void OnHelpButtonClicked()
    {
        SwitchPanel(helpMenuPanel);
    }

    // --- SETTINGS FUNCTIONS ---

    public void OnSettingsExitButtonClicked()
    {
        if (previousPanel != null)
        {
            SwitchPanel(previousPanel);
        }
        else
        {
            SwitchPanel(mainMenuPanel);
        }
    }

    public void OnSettingsBackToMainClicked()
    {
        SwitchPanel(mainMenuPanel);
    }

    public void OnMusicButtonClicked()
    {
        SwitchPanel(musicMenuPanel);
    }

    // --- AUDIO CONTROLS ---

    // For the Slider: Link to "On Value Changed" (Dynamic float)
    public void SetVolume(float volume)
    {
        if (musicSource == null) return;

        musicSource.volume = volume;

        // If player moves slider up while muted, unmute them visually
        if (volume > 0 && isMuted)
        {
            isMuted = false;
            if (muteButtonImage != null) muteButtonImage.sprite = volumeSprite;
        }
    }

    // For the Mute Button: Link to "On Click"
    public void ToggleMute()
    {
        if (musicSource == null) return;

        isMuted = !isMuted;

        if (isMuted)
        {
            preMuteVolume = musicSource.volume; // Save current level
            musicSource.volume = 0;
            if (volumeSlider != null) volumeSlider.value = 0;
            if (muteButtonImage != null) muteButtonImage.sprite = muteSprite;
        }
        else
        {
            // Restore volume or default to 50%
            musicSource.volume = preMuteVolume > 0 ? preMuteVolume : 0.5f;
            if (volumeSlider != null) volumeSlider.value = musicSource.volume;
            if (muteButtonImage != null) muteButtonImage.sprite = volumeSprite;
        }
    }

    // --- MUSIC MENU FUNCTIONS ---

    public void OnOption1Clicked()
    {
        PlaySong(chillyMusic);
    }

    public void OnOption2Clicked()
    {
        PlaySong(dreamMusic);
    }

    private void PlaySong(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        if (musicSource.clip == clip) return; 

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void OnMusicExitButtonClicked()
    {
        SwitchPanel(settingsMenuPanel);
    }

    // --- OTHER FUNCTIONS ---

    public void OnMiniGameButtonClicked()
    {
        SceneManager.LoadScene("mini game scene");
    }

    // --- CORE LOGIC ---

    private void SwitchPanel(GameObject targetPanel)
    {
        // 1. Record history before hiding
        if (mainMenuPanel.activeSelf) previousPanel = mainMenuPanel;
        else if (shopCataloguePanel.activeSelf) previousPanel = shopCataloguePanel;
        else if (helpMenuPanel.activeSelf) previousPanel = helpMenuPanel;

        // 2. Turn all panels off
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        shopCataloguePanel.SetActive(false);
        helpMenuPanel.SetActive(false);
        musicMenuPanel.SetActive(false);

        // 3. Turn requested panel on
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
    }
}