using UnityEngine;

public class AutoDestroyAfterAnim : MonoBehaviour
{
    [SerializeField] private float fallbackLifetime = 1.0f;

    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
            if (clips != null && clips.Length > 0)
            {
                float clipLen = clips[0].clip.length;
                Destroy(gameObject, clipLen);
                return;
            }
        }

        Destroy(gameObject, fallbackLifetime);
    }
}

