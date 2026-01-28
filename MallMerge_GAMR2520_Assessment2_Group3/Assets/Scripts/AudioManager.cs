using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource; 

    [Header("Audio Clips")]
    public AudioClip energyCollectedSFX;
    public AudioClip successSFX;
    public AudioClip itemChestSFX;
    public AudioClip explosionSFX;
    public AudioClip moneySFX;
    public AudioClip swipeSFX;
    public AudioClip winnerSFX;
    public AudioClip resultsPopupSFX;
    public AudioClip chillyMusic; 
    public AudioClip dreamMusic;  
    public AudioClip buttonClickSFX;

    [Header("Volume UI")]
    public Slider musicSlider;    
    public Slider sfxSlider;      
    public Image muteButtonImage;
    public Sprite volumeSprite;
    public Sprite muteSprite;

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private bool isMuted = false;

    void Awake()
    {
        Debug.Log("AudioManager: Awake() called");
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            Debug.Log($"AudioManager: Created singleton instance | musicSource={musicSource != null} | sfxSource={sfxSource != null}");
            
            if (musicSource == null || sfxSource == null)
            {
                Debug.Log("AudioManager: Auto-assigning AudioSources...");
                var sources = GetComponents<AudioSource>();
                Debug.Log($"AudioManager: Found {sources.Length} AudioSource components");
                if (sources.Length > 0 && sfxSource == null) sfxSource = sources[0];
                if (sources.Length > 1 && musicSource == null) musicSource = sources[1];
                if (sources.Length == 1 && musicSource == null) musicSource = sources[0];
                Debug.Log($"AudioManager: After auto-assign | musicSource={musicSource != null} | sfxSource={sfxSource != null}");
            }
        }
        else
        {
            Debug.Log("AudioManager: Duplicate instance detected, destroying...");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log("AudioManager: Start() called - SIMPLIFIED DEBUG MODE");
        
        // SIMPLIFIED: Just set volumes to 1 (full volume)
        if (musicSource != null) musicSource.volume = 1f;
        if (sfxSource != null) sfxSource.volume = 1f;
        
        Debug.Log($"AudioManager: Set volumes to 1.0 | musicSource.volume={musicSource?.volume} | sfxSource.volume={sfxSource?.volume}");

        // COMMENTED OUT FOR DEBUGGING:
        // // 1. Load saved volumes
        // musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        // sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        // // 2. Load and Apply Mute State
        // isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        // musicSource.mute = isMuted;
        // sfxSource.mute = isMuted;
        // if (muteButtonImage != null) muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;

        // // 3. Initial Music setup
        // if (musicSource != null)
        // {
        //     int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0);
        //     var clip = (savedMusic == 0) ? chillyMusic : dreamMusic;

        //     // Only start if we don't already have the right track playing
        //     if (musicSource.clip != clip || !musicSource.isPlaying)
        //         PlaySong(clip);
        // }
        // SetupUI(); 
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"AudioManager: OnSceneLoaded - Scene '{scene.name}' loaded");
        
        // Validate AudioSources are still valid
        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError($"AudioManager: AudioSources became null after loading scene '{scene.name}'! Re-assigning...");
            var sources = GetComponents<AudioSource>();
            if (sources.Length > 0 && sfxSource == null) sfxSource = sources[0];
            if (sources.Length > 1 && musicSource == null) musicSource = sources[1];
            if (sources.Length == 1 && musicSource == null) musicSource = sources[0];
        }
        else
        {
            Debug.Log($"AudioManager: AudioSources are valid after scene load | musicSource={musicSource != null} | sfxSource={sfxSource != null}");
        }
        
        // COMMENTED OUT FOR DEBUGGING:
        // musicSlider = null;
        // sfxSlider = null;
        // muteButtonImage = null;
        // SetupUI();

        // if (musicSource != null && !musicSource.isPlaying)
        // {
        //     int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0);
        //     PlaySong(savedMusic == 0 ? chillyMusic : dreamMusic);
        // }
    }

    // COMMENTED OUT FOR DEBUGGING
    // public void SetupUI()
    // {
    //     Debug.Log("AudioManager: SetupUI() called");
    //     
    //     GameObject volumeSliderObj = GameObject.FindGameObjectWithTag("VolumeSliderTag");
    //     if (volumeSliderObj != null)
    //     {
    //         musicSlider = volumeSliderObj.GetComponent<Slider>();
    //         Debug.Log($"AudioManager: Found VolumeSliderTag | musicSlider={musicSlider != null}");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("AudioManager: Could not find GameObject with tag 'VolumeSliderTag'");
    //     }
    //     
    //     if (musicSlider != null && musicSource != null)
    //     {
    //         musicSlider.onValueChanged.RemoveAllListeners();
    //         musicSlider.SetValueWithoutNotify(musicSource.volume);
    //         musicSlider.onValueChanged.AddListener(SetMusicVolume);
    //         Debug.Log($"AudioManager: Setup music slider | volume={musicSource.volume}");
    //     }

    //     GameObject sfxSliderObj = GameObject.FindGameObjectWithTag("SFXSliderTag");
    //     if (sfxSliderObj != null)
    //     {
    //         sfxSlider = sfxSliderObj.GetComponent<Slider>();
    //         Debug.Log($"AudioManager: Found SFXSliderTag | sfxSlider={sfxSlider != null}");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("AudioManager: Could not find GameObject with tag 'SFXSliderTag'");
    //     }

    //     if (sfxSlider != null && sfxSource != null)
    //     {
    //         sfxSlider.onValueChanged.RemoveAllListeners();
    //         sfxSlider.SetValueWithoutNotify(sfxSource.volume);
    //         sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    //         Debug.Log($"AudioManager: Setup SFX slider | volume={sfxSource.volume} | mute={sfxSource.mute}");
    //     }

    //     muteButtonImage = GameObject.Find("Mute_Button")?.GetComponent<Image>();
    //     if (muteButtonImage != null)
    //     {
    //         muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;
    //         Debug.Log($"AudioManager: Found Mute_Button | isMuted={isMuted}");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("AudioManager: Could not find 'Mute_Button'");
    //     }
    // }


    // --- VOLUME CONTROLS (COMMENTED OUT FOR DEBUGGING) ---

    // public void SetMusicVolume(float volume)
    // {
    //     if (musicSource == null) return;
    //     musicSource.volume = volume;
    //     PlayerPrefs.SetFloat("MusicVolume", volume);
    //     PlayerPrefs.Save();
    // }

    // public void SetSFXVolume(float volume)
    // {
    //     if (sfxSource == null) return;
    //     sfxSource.volume = volume;
    //     PlayerPrefs.SetFloat("SFXVolume", volume);
    //     PlayerPrefs.Save();
    // }

    // --- MUTE TOGGLE (COMMENTED OUT FOR DEBUGGING) ---

    // public void ToggleMute()
    // {
    //     isMuted = !isMuted;
    //     
    //     // Save the mute state
    //     PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    //     PlayerPrefs.Save();

    //     // Direct mute property doesn't destroy volume settings
    //     if (musicSource != null) musicSource.mute = isMuted;
    //     if (sfxSource != null) sfxSource.mute = isMuted;

    //     if (muteButtonImage != null)
    //     {
    //         muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;
    //     }

    //     if (!isMuted) PlayClick();
    // }

    // --- SFX PLAYBACK METHODS ---

    public void PlaySFX(AudioClip clip)
    {
        if (AudioManagerV2.Instance != null)
        {
            AudioManagerV2.Instance.PlaySFX(clip);
        }
        else
        {
            Debug.LogError("AudioManager: AudioManagerV2.Instance is NULL!");
        }
    }

    public void PlayClick() => PlaySFX(buttonClickSFX);
    public void PlayEnergy() => PlaySFX(energyCollectedSFX);
    public void PlaySuccess() => PlaySFX(successSFX);
    public void PlayItemChest() => PlaySFX(itemChestSFX);
    public void PlayExplosion() => PlaySFX(explosionSFX);
    public void PlayMoney() => PlaySFX(moneySFX);
    public void PlaySwipe() => PlaySFX(swipeSFX);
    public void PlayWinner() => PlaySFX(winnerSFX);
    public void PlayResultsPopup() => PlaySFX(resultsPopupSFX);

    public void PlayClickSound() => PlayClick();

    // --- MUSIC SELECTION ---

    public void OnOption1Clicked() { PlayerPrefs.SetInt("SelectedMusic", 0); PlaySong(chillyMusic); }
    public void OnOption2Clicked() { PlayerPrefs.SetInt("SelectedMusic", 1); PlaySong(dreamMusic); }

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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // Debug method to check AudioManager health
    private void Update()
    {
        // Press 'D' key to debug AudioManager status
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("=== AudioManager Debug Info ===");
            Debug.Log($"Instance exists: {instance != null}");
            Debug.Log($"Music Source: {(musicSource != null ? "Valid" : "NULL")}");
            Debug.Log($"SFX Source: {(sfxSource != null ? "Valid" : "NULL")}");
            Debug.Log($"Music Volume: {(musicSource != null ? musicSource.volume.ToString() : "N/A")}");
            Debug.Log($"SFX Volume: {(sfxSource != null ? sfxSource.volume.ToString() : "N/A")}");
            Debug.Log($"Is Muted: {isMuted}");
            Debug.Log($"Music Muted: {(musicSource != null ? musicSource.mute.ToString() : "N/A")}");
            Debug.Log($"SFX Muted: {(sfxSource != null ? sfxSource.mute.ToString() : "N/A")}");
            Debug.Log($"Music Playing: {(musicSource != null ? musicSource.isPlaying.ToString() : "N/A")}");
            Debug.Log($"GameObject name: {gameObject.name}");
            Debug.Log($"AudioSource count: {GetComponents<AudioSource>().Length}");
        }
    }
}