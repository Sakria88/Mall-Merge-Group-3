using UnityEngine;

/// <summary>
/// Handles chest clicks using the new merge system
/// </summary>
public class ChestButton : MonoBehaviour
{
    public MergeBoardController board; // drag in inspector
    public ChestFamily family;         // set per chest

    /// <summary>
    /// Called by the Button OnClick()
    /// </summary>
    public void OnChestClicked()
    {
        board.SpawnIntoRandomEmpty(family, 0);
    }
}
