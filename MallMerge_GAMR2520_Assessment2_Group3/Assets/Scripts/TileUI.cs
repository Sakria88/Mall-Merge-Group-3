using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class TileUI : MonoBehaviour
{
    //Grid positions
    public int gridi; //Column
    public int gridj; //Row

    //UI 
    //public Image backgroundImage;
    public GameObject currentItem;//The item (fruit, tech etc) on the tile

    /// <summary>
    /// setting the value of the tile and its position
    /// </summary>

    public void Init(int i, int j)
    {
        gridi = i;
        gridj = j;

       currentItem = null;
        //RefreshUI();
        Debug.Log($"Tile initialized at ({gridi}, {gridj})");
    }

    /// <summary>
    /// Placing Items on the tile
    /// </summary>
    /// 
    //public void SetItem()
    //{
    //    //Removing any items on the tile
    //    if(currentItem != null)
    //    {
    //        Destroy(currentItem);
    //    }

    //    //
    //}

}
