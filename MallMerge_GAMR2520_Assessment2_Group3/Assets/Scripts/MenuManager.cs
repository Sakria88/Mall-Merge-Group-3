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
    public GameObject pauseMenuPanel; // Added Pause Menu reference

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

    // --- PERSISTENCE LOGIC ---
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
    }

    // --- SCENE EVENT REGISTRATION ---
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MallMergeMenus")
        {
            SetupUI();
        }
    }

    void Start()
    {
        if (musicSource != null)
        {
            if (musicSource.volume <= 0) musicSource.volume = 0.5f;

            if (musicSource.clip == null || !musicSource.isPlaying)
            {
                int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0); 
                AudioClip clipToPlay = (savedMusic == 0) ? chillyMusic : dreamMusic;
                PlaySong(clipToPlay);
            }
        }

        SetupUI(); 
    }

    // Reassigns panels by searching the hierarchy by name
    private void ReassignPanels()
    {
        // If we are in the Menu scene, find the panels in the hierarchy
        if (SceneManager.GetActiveScene().name == "MallMergeMenus")
        {
            // Note: Make sure these names match your Hierarchy exactly!
            if (mainMenuPanel == null) mainMenuPanel = GameObject.Find("MainMenu_Panel");
            if (settingsMenuPanel == null) settingsMenuPanel = GameObject.Find("Settings_Panel");
            if (shopCataloguePanel == null) shopCataloguePanel = GameObject.Find("ShopCatalogue_Panel");
            if (helpMenuPanel == null) helpMenuPanel = GameObject.Find("HelpMenu_Panel");
            if (musicMenuPanel == null) musicMenuPanel = GameObject.Find("MusicMenu_Panel");
            if (pauseMenuPanel == null) pauseMenuPanel = GameObject.Find("PauseMenu_Panel");
        }
    }

    // --- BULLETPROOF SETUP UI (Including Pause Menu Fix) ---
    private void SetupUI()
    {
        Debug.Log("MenuManager: Running hard reset on UI panels...");

        // Call the reassignment logic to find objects in the current scene
        ReassignPanels();

        // EMERGENCY HIDE: Force every panel to inactive immediately
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);
        if (shopCataloguePanel != null) shopCataloguePanel.SetActive(false);
        if (helpMenuPanel != null) helpMenuPanel.SetActive(false);
        if (musicMenuPanel != null) musicMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        // Volume Slider check
        if (volumeSlider == null) volumeSlider = GameObject.Find("Volume_Slider")?.GetComponent<Slider>();
        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
        }

        // THE DECISION: Which one stays open?
        if (!string.IsNullOrEmpty(targetPanelName))
        {
            Debug.Log("MenuManager: Navigation target found: " + targetPanelName);
            
            if (targetPanelName.Equals("Settings_Panel", System.StringComparison.OrdinalIgnoreCase)) 
                instance.SwitchPanel(settingsMenuPanel);
            
            else if (targetPanelName.Equals("ShopCatalogue_Panel", System.StringComparison.OrdinalIgnoreCase)) 
                instance.SwitchPanel(shopCataloguePanel);
            
            else if (targetPanelName.Equals("HelpMenu_Panel", System.StringComparison.OrdinalIgnoreCase)) 
                instance.SwitchPanel(helpMenuPanel);
            
            targetPanelName = ""; 
        }
        else
        {
            if (mainMenuPanel != null) 
            {
                instance.SwitchPanel(mainMenuPanel);
                Debug.Log("MenuManager: Defaulting to MainMenu_Panel");
            }
            else
            {
                Debug.LogError("MenuManager ERROR: Could not find MainMenu_Panel in the Hierarchy!");
            }
        }
    }

    // --- BUTTON AND NAVIGATION METHODS ---
    public void PlayClickSound()
    {
        if (musicSource != null && buttonClickSFX != null)
        {
            musicSource.PlayOneShot(buttonClickSFX);
        }
    }

    public void OnPlayButtonClicked()
    {
        instance.PlayClickSound();
        SceneManager.LoadScene("MainGamePlayArea");
    }

    public void OnSettingsButtonClicked() 
    { 
        instance.PlayClickSound(); 
        instance.SwitchPanel(settingsMenuPanel); 
    }

    public void OnShopButtonClicked() 
    { 
        instance.PlayClickSound(); 
        instance.SwitchPanel(shopCataloguePanel); 
    }

    public void OnHelpButtonClicked() 
    { 
        instance.PlayClickSound(); 
        instance.SwitchPanel(helpMenuPanel); 
    }

    public void BackToMainMenu()
    {
        instance.PlayClickSound();
        instance.SwitchPanel(mainMenuPanel);
    }

    public void OnSettingsExitButtonClicked() { UniversalExit(); }
    public void OnMusicButtonClicked() { instance.PlayClickSound(); instance.SwitchPanel(instance.musicMenuPanel); }
    public void OnMusicExitButtonClicked() { instance.PlayClickSound(); instance.SwitchPanel(instance.settingsMenuPanel); }

    public void OnHelpExitButtonClicked()
    {
        instance.PlayClickSound();
        if (instance.previousPanel != null) instance.SwitchPanel(instance.previousPanel);
        else SceneManager.LoadScene("MainGamePlayArea");
    }

    public void UniversalExit()
    {
        instance.PlayClickSound();
        if (instance.previousPanel != null && instance.previousPanel != instance.musicMenuPanel) instance.SwitchPanel(instance.previousPanel);
        else instance.SwitchPanel(instance.mainMenuPanel);
    }

    // --- AUDIO CONTROLS ---
    public void SetVolume(float volume)
    {
        if (musicSource == null) return;
        musicSource.volume = volume;
    }

    public void ToggleMute()
    {
        if (musicSource == null) return;
        instance.PlayClickSound();
        isMuted = !isMuted;

        if (isMuted)
        {
            preMuteVolume = musicSource.volume;
            musicSource.volume = 0f;
        }
        else
        {
            musicSource.volume = preMuteVolume;
        }
    }

    public void OnOption1Clicked()
    {
        PlayerPrefs.SetInt("SelectedMusic", 0);
        PlaySong(chillyMusic);
    }

    public void OnOption2Clicked()
    {
        PlayerPrefs.SetInt("SelectedMusic", 1);
        PlaySong(dreamMusic);
    }

    private void PlaySong(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        if (musicSource.clip != clip || !musicSource.isPlaying)
        {
            musicSource.clip = clip;
            musicSource.Play();
            musicSource.loop = true;
        }
    }

    public void OnMiniGameButtonClicked()
    {
        instance.PlayClickSound();
        SceneManager.LoadScene("MiniGameScene");
    }

    private void SwitchPanel(GameObject targetPanel)
    {
        if (targetPanel == null) 
        {
            Debug.LogError("MenuManager: SwitchPanel failed because targetPanel is NULL!");
            return;
        }

        // Track previous panel
        if (mainMenuPanel != null && mainMenuPanel.activeSelf) previousPanel = mainMenuPanel;
        else if (shopCataloguePanel != null && shopCataloguePanel.activeSelf) previousPanel = shopCataloguePanel;
        else if (helpMenuPanel != null && helpMenuPanel.activeSelf) previousPanel = helpMenuPanel;

        // Hide absolutely everything
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);
        if (shopCataloguePanel != null) shopCataloguePanel.SetActive(false);
        if (helpMenuPanel != null) helpMenuPanel.SetActive(false);
        if (musicMenuPanel != null) musicMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        targetPanel.SetActive(true);
        Debug.Log("MenuManager: Successfully switched to " + targetPanel.name);
    }
}