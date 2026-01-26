using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum that identifies which chest family an item belongs to
/// </summary>
public enum ChestFamily
{
    FruitGreen,
    DessertYellow,
    WardrobePurple,
    MakeupPink,
    TechBlue
}

/// <summary>
/// ScriptableObject that stores the merge order (sprite chain) for one chest
/// Example: blueberry -> strawberry -> peach -> apple -> pineapple
/// </summary>
[CreateAssetMenu(menuName = "Merge Game/Merge Chain Data")]
public class MergeChainData : ScriptableObject
{
    public ChestFamily family;

    // List of sprites in merge order (level 0 to max)
    public List<Sprite> levelSprites = new List<Sprite>(5);

    // Highest merge level allowed
    public int MaxLevel => levelSprites.Count - 1;

    // Returns sprite for given level
    public Sprite GetSprite(int level)
    {
        if (level < 0 || level >= levelSprites.Count) return null;
        return levelSprites[level];
    }
}
