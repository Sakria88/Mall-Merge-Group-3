using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to every mergeable item prefab.
/// Stores its family (which chest) and its current merge level.
/// </summary>
public class MergeItem : MonoBehaviour
{
    public ChestFamily family; // Which chest it belongs to
    public int level;          // 0 = first sprite, 1 = second sprite, etc

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    /// <summary>
    /// Sets the sprite and data based on chain + level
    /// </summary>
    public void ApplyVisual(MergeChainData chain, ChestFamily fam, int lvl)
    {
        family = fam;
        level = lvl;

        if (img == null)
            img = GetComponent<Image>();

        Sprite s = chain.GetSprite(level);
        if (s != null)
            img.sprite = s;
    }
}
