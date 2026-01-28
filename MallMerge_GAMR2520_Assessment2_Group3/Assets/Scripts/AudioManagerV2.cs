using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManagerV2 : MonoBehaviour
{
    public static AudioManagerV2 Instance { get; private set; }
    
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
    
    private AudioSource musicSource;
    private AudioSource sfxSource;
    
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string IS_MUTED_KEY = "IsMuted";
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("AudioManagerV2: Duplicate detected, destroying this instance");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            musicSource = sources[0];
            sfxSource = sources[1];
            
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            
            LoadVolumeSettings();
            
            Debug.Log($"AudioManagerV2: Initialized | musicSource={musicSource != null} | sfxSource={sfxSource != null}");
        }
        else
        {
            Debug.LogError($"AudioManagerV2: Need 2 AudioSource components, found {sources.Length}");
        }
    }
    
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        if (musicSource != null && chillyMusic != null)
        {
            musicSource.clip = chillyMusic;
            musicSource.Play();
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"AudioManagerV2: Scene '{scene.name}' loaded | musicSource={musicSource != null} | sfxSource={sfxSource != null}");
        
        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError("AudioManagerV2: AudioSources lost! Re-initializing...");
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length >= 2)
            {
                musicSource = sources[0];
                sfxSource = sources[1];
            }
        }
        
        UpdateSlidersInScene();
    }
    
    void LoadVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);
        
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
            musicSource.mute = false;
        }
        
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
            sfxSource.mute = false;
        }
        
        PlayerPrefs.SetInt(IS_MUTED_KEY, 0);
        PlayerPrefs.Save();
        
        Debug.Log($"AudioManagerV2: Loaded settings | Music={musicVolume} | SFX={sfxVolume} | Muted=False (always unmuted on start)");
    }
    
    void UpdateSlidersInScene()
    {
        Slider[] volumeSliders = GameObject.FindGameObjectsWithTag("VolumeSliderTag")
            .Select(go => go.GetComponent<Slider>())
            .Where(s => s != null)
            .ToArray();
            
        Slider[] sfxSliders = GameObject.FindGameObjectsWithTag("SFXSliderTag")
            .Select(go => go.GetComponent<Slider>())
            .Where(s => s != null)
            .ToArray();
        
        foreach (var slider in volumeSliders)
        {
            slider.value = musicSource.volume;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(SetMusicVolume);
        }
        
        foreach (var slider in sfxSliders)
        {
            slider.value = sfxSource.volume;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(SetSFXVolume);
        }
        
        Debug.Log($"AudioManagerV2: Updated {volumeSliders.Length} volume sliders and {sfxSliders.Length} SFX sliders");
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null)
        {
            Debug.LogError("AudioManagerV2: sfxSource is NULL!");
            return;
        }
        
        if (clip == null)
        {
            Debug.LogWarning("AudioManagerV2: AudioClip is NULL!");
            return;
        }
        
        Debug.Log($"AudioManagerV2: Playing '{clip.name}' | volume={sfxSource.volume} | mute={sfxSource.mute}");
        sfxSource.PlayOneShot(clip);
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
    
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
            PlayerPrefs.Save();
            Debug.Log($"AudioManagerV2: Music volume set to {volume}");
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
            PlayerPrefs.Save();
            Debug.Log($"AudioManagerV2: SFX volume set to {volume}");
        }
    }
    
    public void ToggleMute()
    {
        bool newMuteState = !sfxSource.mute;
        if (musicSource != null) musicSource.mute = newMuteState;
        if (sfxSource != null) sfxSource.mute = newMuteState;
        PlayerPrefs.SetInt(IS_MUTED_KEY, newMuteState ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"AudioManagerV2: Mute toggled to {newMuteState}");
    }
}
