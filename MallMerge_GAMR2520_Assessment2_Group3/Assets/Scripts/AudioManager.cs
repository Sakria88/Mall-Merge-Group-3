using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
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
    private static AudioManager instance;

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

    // --- BULLETPROOF SETUP UI (Including Pause Menu Fix) ---
    private void SetupUI()
    {
    
        // Volume Slider check
        if (volumeSlider == null) volumeSlider = GameObject.Find("Volume_Slider")?.GetComponent<Slider>();
        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
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

}