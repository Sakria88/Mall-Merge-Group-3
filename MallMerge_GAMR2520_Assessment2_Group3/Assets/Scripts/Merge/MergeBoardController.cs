using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Swipe directions
/// </summary>
/// 


public enum SwipeDir { Up, Down, Left, Right }


/// <summary>
/// Controls moving and merging all items on the grid
/// </summary>
public class MergeBoardController : MonoBehaviour
{
    // Reference to OrderManager so we can tell it when merges happen
    public OrderManager orderManager;

    public GridManager gridManager; // your existing grid
    public ItemPool itemPool;       // object pool

    // Assign 5 chain assets (FruitChain, DessertChain, etc.)
    public List<MergeChainData> chains;

    private Dictionary<ChestFamily, MergeChainData> chainLookup;

    private void Awake()
    {
        chainLookup = new Dictionary<ChestFamily, MergeChainData>();

        if (chains == null)
        {
            Debug.LogError("MergeBoardController: Chains list is NULL.");
            return;
        }

        foreach (var c in chains)
        {
            if (c == null)
            {
                Debug.LogError("MergeBoardController: A Chains element is NONE (null). Fill all 5 chain assets.");
                continue;
            }

            chainLookup[c.family] = c;
        }
    }


    /// <summary>
    /// Called by swipe input
    /// </summary>
    public void Move(SwipeDir dir)
    {
        bool[,] mergedThisMove = new bool[gridManager.columns, gridManager.rows];

        // Set traversal order like 2048
        int xStart = 0, xEnd = gridManager.columns, xStep = 1;
        int yStart = 0, yEnd = gridManager.rows, yStep = 1;

        if (dir == SwipeDir.Right) { xStart = gridManager.columns - 1; xEnd = -1; xStep = -1; }
        if (dir == SwipeDir.Up) { yStart = gridManager.rows - 1; yEnd = -1; yStep = -1; }

        for (int y = yStart; y != yEnd; y += yStep)
        {
            for (int x = xStart; x != xEnd; x += xStep)
            {
                TileUI tile = gridManager.GetTileUI(x, y);
                if (tile == null || tile.currentItem == null) continue;

                SlideAndMerge(tile, dir, mergedThisMove);
            }
        }
    }

    /// <summary>
    /// Moves one item until blocked, and merges if possible
    /// </summary>
    private void SlideAndMerge(TileUI fromTile, SwipeDir dir, bool[,] mergedThisMove)
    {
        GameObject movingObj = fromTile.currentItem;
        MergeItem movingItem = movingObj.GetComponent<MergeItem>();

        int dx = 0, dy = 0;
        if (dir == SwipeDir.Left) dx = -1;
        if (dir == SwipeDir.Right) dx = 1;
        if (dir == SwipeDir.Down) dy = -1;
        if (dir == SwipeDir.Up) dy = 1;

        int x = fromTile.gridi;
        int y = fromTile.gridj;

        TileUI lastEmpty = null;

        while (true)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (!gridManager.InBounds(nx, ny)) break;

            TileUI nextTile = gridManager.GetTileUI(nx, ny);

            if (nextTile.currentItem == null)
            {
                lastEmpty = nextTile;
                x = nx;
                y = ny;
                continue;
            }

            // Check merge
            MergeItem targetItem = nextTile.currentItem.GetComponent<MergeItem>();

            if (targetItem.family == movingItem.family &&
                targetItem.level == movingItem.level &&
                mergedThisMove[nx, ny] == false &&
                movingItem.level < chainLookup[movingItem.family].MaxLevel)
            {
                DoMerge(fromTile, nextTile, movingItem);
                mergedThisMove[nx, ny] = true;
            }

            return;
        }

        if (lastEmpty != null)
        {
            MoveItem(fromTile, lastEmpty, movingObj);
        }
    }

    /// <summary>
    /// Performs a merge between two items
    /// </summary>
    private void DoMerge(TileUI fromTile, TileUI toTile, MergeItem movingItem)
    {
        MergeChainData chain = chainLookup[movingItem.family];

        fromTile.currentItem = null;

        // Return consumed item to pool
        itemPool.Return(movingItem.family, movingItem.gameObject);

        // Upgrade the target
        MergeItem target = toTile.currentItem.GetComponent<MergeItem>();
        int newLevel = target.level + 1;
        target.ApplyVisual(chain, target.family, newLevel);

        OrderManager orderManager = FindObjectOfType<OrderManager>();
        if (orderManager != null)
            orderManager.CheckForDeliveries();
    }

    /// <summary>
    /// Moves item to another tile
    /// </summary>
    private void MoveItem(TileUI fromTile, TileUI toTile, GameObject obj)
    {
        fromTile.currentItem = null;
        toTile.currentItem = obj;

        obj.transform.SetParent(toTile.transform, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Used by chest buttons to spawn items
    /// </summary>
    public void SpawnIntoRandomEmpty(ChestFamily family, int startLevel)
    {
        if (!gridManager.TryGetRandomEmptyTile(out Vector2Int pos)) return;

        TileUI tile = gridManager.GetTileUI(pos.x, pos.y);

        GameObject obj = itemPool.Get(family);
        obj.transform.SetParent(tile.transform, false);
        obj.transform.localPosition = Vector3.zero;

        MergeItem mi = obj.GetComponent<MergeItem>();
        mi.ApplyVisual(chainLookup[family], family, startLevel);

        tile.currentItem = obj;
        gridManager.OccupyTile(pos.x, pos.y);

        if (orderManager != null)
            orderManager.CheckForDeliveries();
    }
}
