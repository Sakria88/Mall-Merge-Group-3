using UnityEngine;

public class PlaySFXOnEnable : MonoBehaviour
{
    public enum SFXType
    {
        Explosion,
        Success
    }

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SFXType sfxToPlay;

    private bool hasPlayed = false;

    private void OnEnable()
    {
        if (hasPlayed) return;
        hasPlayed = true;

        if (audioManager == null) return;

        switch (sfxToPlay)
        {
            case SFXType.Explosion:
                audioManager.PlayExplosion();
                break;

            case SFXType.Success:
                audioManager.PlaySFX(audioManager.successSFX);
                // or audioManager.PlaySuccessSFX();
                break;
        }
    }

    private void OnDisable()
    {
        hasPlayed = false;
    }
}
