using UnityEngine;

[System.Obsolete("AudioManager is deprecated. Use AudioManagerV2 instead.")]
public class AudioManagerLegacy : MonoBehaviour
{
    private static AudioManagerLegacy instance;
    public static AudioManagerLegacy Instance => instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManagerLegacy: Wrapper - all calls forwarded to AudioManagerV2");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (AudioManagerV2.Instance != null)
            AudioManagerV2.Instance.PlaySFX(clip);
    }
    
    public void PlayClick() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayClick(); }
    public void PlayEnergy() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayEnergy(); }
    public void PlaySuccess() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlaySuccess(); }
    public void PlayItemChest() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayItemChest(); }
    public void PlayExplosion() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayExplosion(); }
    public void PlayMoney() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayMoney(); }
    public void PlaySwipe() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlaySwipe(); }
    public void PlayWinner() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayWinner(); }
    public void PlayResultsPopup() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayResultsPopup(); }
    public void PlayClickSound() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.PlayClickSound(); }
    
    public void SetMusicVolume(float volume) { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.SetMusicVolume(volume); }
    public void SetSFXVolume(float volume) { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.SetSFXVolume(volume); }
    public void ToggleMute() { if (AudioManagerV2.Instance != null) AudioManagerV2.Instance.ToggleMute(); }
    
    public void OnOption1Clicked() { }
    public void OnOption2Clicked() { }
}
