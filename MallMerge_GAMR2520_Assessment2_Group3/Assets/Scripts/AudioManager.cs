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

        if (musicSource == null || sfxSource == null)
        {
        var sources = GetComponents<AudioSource>();
        if (sources.Length > 0 && sfxSource == null) sfxSource = sources[0];
        if (sources.Length > 1 && musicSource == null) musicSource = sources[1];
        if (sources.Length == 1 && musicSource == null) musicSource = sources[0];
        }
    }

    void Start()
    {
        // 1. Load saved volumes
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        // 2. Load and Apply Mute State
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        musicSource.mute = isMuted;
        sfxSource.mute = isMuted;
        if (muteButtonImage != null) muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;

        // 3. Initial Music setup
        if (musicSource != null)
        {
            int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0);
            var clip = (savedMusic == 0) ? chillyMusic : dreamMusic;

            // Only start if we don't already have the right track playing
            if (musicSource.clip != clip || !musicSource.isPlaying)
                PlaySong(clip);
        }
        SetupUI(); 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    SetupUI();

    // Resume music if not playing
    if (musicSource != null && !musicSource.isPlaying)
    {
        int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0);
        PlaySong(savedMusic == 0 ? chillyMusic : dreamMusic);
    }
    
    }

    public void SetupUI()
    {
        if (musicSlider == null) 
            musicSlider = GameObject.Find("Volume_Slider")?.GetComponent<Slider>();
        
        if (musicSlider != null && musicSource != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider == null) 
            sfxSlider = GameObject.Find("SFX_Slider")?.GetComponent<Slider>();

        if (sfxSlider != null && sfxSource != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.value = sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // --- IMPROVED VOLUME CONTROLS ---

    public void SetMusicVolume(float volume)
    {
        if (musicSource == null) return;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        
        // If moving slider while muted, unmute automatically
        if (volume > 0 && isMuted) ToggleMute(); 
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource == null) return;
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();

        if (volume > 0 && isMuted) ToggleMute();
    }

    // --- IMPROVED MUTE TOGGLE ---

    public void ToggleMute()
    {
        isMuted = !isMuted;
        
        // Save the mute state
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Direct mute property doesn't destroy volume settings
        if (musicSource != null) musicSource.mute = isMuted;
        if (sfxSource != null) sfxSource.mute = isMuted;

        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;
        }

        if (!isMuted) PlayClick();
    }

    // --- SFX PLAYBACK METHODS ---

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
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
}