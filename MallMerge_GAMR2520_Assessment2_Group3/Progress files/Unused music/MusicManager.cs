using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Components")]
    public AudioSource musicSource;
    public AudioClip chillyMusic;
    public AudioClip dreamMusic;

    void Start()
    {
        // 1. Read the saved choice from PlayerPrefs
        int savedMusic = PlayerPrefs.GetInt("SelectedMusic", 0);

        // 2. Play the corresponding clip based on that saved value
        if (savedMusic == 0)
        {
            PlaySong(chillyMusic);
        }
        else
        {
            PlaySong(dreamMusic);
        }
    }

    private void PlaySong(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
            musicSource.loop = true;
        }
    }
}