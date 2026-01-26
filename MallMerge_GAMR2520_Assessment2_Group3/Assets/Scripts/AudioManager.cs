using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource; 

    [Header("Audio Clips")]
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
    
    // This allows buttons to find the manager easily
    public static AudioManager Instance => instance;

    private bool isMuted = false;
    private float preMuteMusicVol = 1f;
    private float preMuteSFXVol = 1f;

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

    void Start()
    {
        // Load saved volumes
        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSFXVol = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        if (musicSource != null) musicSource.volume = savedMusicVol;
        if (sfxSource != null) sfxSource.volume = savedSFXVol;

        // Initial Music setup
        if (musicSource != null && !musicSource.isPlaying)
        {
            int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0); 
            PlaySong(savedMusic == 0 ? chillyMusic : dreamMusic);
        }

        SetupUI(); 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupUI();
    }

    // Call this via your Menu Script whenever the Settings Panel is opened!
    public void SetupUI()
    {
        // Find Music Slider
        if (musicSlider == null) 
            musicSlider = GameObject.Find("Volume_Slider")?.GetComponent<Slider>();
        
        if (musicSlider != null && musicSource != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // Find SFX Slider
        if (sfxSlider == null) 
            sfxSlider = GameObject.Find("SFX_Slider")?.GetComponent<Slider>();

        if (sfxSlider != null && sfxSource != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.value = sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource == null) return;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource == null) return;
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void PlayClickSound()
    {
        if (sfxSource != null && buttonClickSFX != null)
        {
            sfxSource.PlayOneShot(buttonClickSFX);
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayClickSound();

        if (isMuted)
        {
            preMuteMusicVol = musicSource.volume;
            preMuteSFXVol = sfxSource.volume;
            SetMusicVolume(0f);
            SetSFXVolume(0f);
        }
        else
        {
            SetMusicVolume(preMuteMusicVol > 0 ? preMuteMusicVol : 0.5f);
            SetSFXVolume(preMuteSFXVol > 0 ? preMuteSFXVol : 0.7f);
        }

        if (musicSlider != null) musicSlider.value = musicSource.volume;
        if (sfxSlider != null) sfxSlider.value = sfxSource.volume;
    }

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