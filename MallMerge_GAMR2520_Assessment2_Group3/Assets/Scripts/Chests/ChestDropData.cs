using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds drop data for a chest:
/// - Which items can drop
/// - The percentage chance for each
/// </summary>

[System.Serializable]
public class DropItem
{
    // Prefab to spawn
    public GameObject prefab;

    // Chance (out of 100) for this item to drop
    [Range(0f, 100f)]
    public float dropChance;
}

[CreateAssetMenu(menuName = "Merge Game/Chest Drop Data")]
public class ChestDropData : ScriptableObject
{
    // List of possible drops for this chest
    public DropItem[] items;
}
