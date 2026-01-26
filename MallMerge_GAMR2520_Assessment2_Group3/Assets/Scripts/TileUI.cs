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

    /// <summary>
    /// Places an item neatly into this tile (UI snap)
    /// </summary>
    public void PlaceItem(GameObject item)
    {
        currentItem = item;

        // Parent to the tile so it sits inside the correct cell
        item.transform.SetParent(transform, false);

        RectTransform r = item.GetComponent<RectTransform>();
        if (r != null)
        {
            // Center in tile
            r.anchorMin = new Vector2(0.5f, 0.5f);
            r.anchorMax = new Vector2(0.5f, 0.5f);
            r.pivot = new Vector2(0.5f, 0.5f);

            r.anchoredPosition = Vector2.zero;
            r.localScale = Vector3.one;
            r.localRotation = Quaternion.identity;
        }

        // Render above tile graphics
        item.transform.SetAsLastSibling();
    }
    /// <summary>
/// Clears the tile's reference (does not destroy item)
/// </summary>
public void ClearItem()
{
    currentItem = null;
}

}
