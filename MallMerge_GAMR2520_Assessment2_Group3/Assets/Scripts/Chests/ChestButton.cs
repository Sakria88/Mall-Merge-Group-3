using UnityEngine;

public class ChestButton : MonoBehaviour
{
    public MergeBoardController board;
    public ChestFamily family;
    public int startLevel = 0;

   

    public void OnChestClicked()
    {
        board.SpawnIntoRandomEmpty(family, startLevel);

    
    }
}
