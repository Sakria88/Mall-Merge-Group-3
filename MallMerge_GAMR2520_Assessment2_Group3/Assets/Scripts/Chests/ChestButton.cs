using UnityEngine;

public class ChestButton : MonoBehaviour
{
    public MergeBoardController board;
    public ChestFamily family;
    public int startLevel = 0;

    public AudioSource chestSound; // 🔊 add this

    public void OnChestClicked()
    {
        board.SpawnIntoRandomEmpty(family, startLevel);

        // Play sound when item comes out
        if (chestSound != null)
            chestSound.PlayOneShot(chestSound.clip);
    }
}
