using UnityEngine;

/// <summary>
/// Handles what happens when a chest button is clicked.
/// This script:
/// - Chooses a random item based on drop chances
/// - Finds a random empty tile from the correct GridManager
/// - Spawns the item into that tile
/// </summary>
public class ChestButton : MonoBehaviour
{
    // Reference to the GridManager for THIS area (e.g. Area 5)
    // This is assigned in the Inspector
    public GridManager gridManager;

    // The drop table (ScriptableObject) that defines what this chest can spawn
    public ChestDropData dropData;

    /// <summary>
    /// This function is called when the chest button is clicked
    /// (via the Button OnClick event in the Inspector)
    /// </summary>
    public void OnChestClicked()
    {
        Debug.Log("Chest clicked");

        // Safety check: make sure a GridManager is assigned
        if (gridManager == null)
        {
            Debug.LogError("No GridManager assigned to this chest!");
            return;
        }

        // Ask THIS GridManager for a random empty tile
        if (!gridManager.TryGetRandomEmptyTile(out Vector2Int tilePos))
        {
            Debug.Log("No empty tiles available");
            return;
        }

        // Get a random item prefab based on drop chances
        GameObject itemToSpawn = GetRandomDrop();

        // Safety check: make sure an item was returned
        if (itemToSpawn == null)
        {
            Debug.LogError("GetRandomDrop returned null. Check ChestDropData setup.");
            return;
        }

        // Get the transform of the chosen tile from THIS GridManager
        Transform tile = gridManager.GetTileTransform(tilePos.x, tilePos.y);

        // Instantiate (spawn) the item prefab
        GameObject spawnedItem = Instantiate(itemToSpawn);

        // Make the item a child of the tile (so it appears in the grid)
        spawnedItem.transform.SetParent(tile, false);

        // Center the item inside the tile
        spawnedItem.transform.localPosition = Vector3.zero;

        // Make sure the scale is correct
        spawnedItem.transform.localScale = Vector3.one;

        // Ensure the item renders above the tile image
        spawnedItem.transform.SetAsLastSibling();

        // Mark this tile as occupied so it can't be used again
        gridManager.OccupyTile(tilePos.x, tilePos.y);

        Debug.Log($"Item spawned into tile ({tilePos.x}, {tilePos.y})");
    }

    /// <summary>
    /// Chooses a random item from the ChestDropData using weighted probability
    /// </summary>
    private GameObject GetRandomDrop()
    {
        // Generate a random number between 0 and 100
        float roll = Random.Range(0f, 100f);

        float cumulative = 0f;

        // Loop through all possible drop items
        foreach (var item in dropData.items)
        {
            cumulative += item.dropChance;

            // If the roll falls within this item's chance, return it
            if (roll <= cumulative)
                return item.prefab;
        }

        // If something goes wrong, return null
        return null;
    }
}
