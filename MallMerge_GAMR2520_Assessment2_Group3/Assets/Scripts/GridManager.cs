using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
/// <summary>
/// 5x5 Grid for the merge game
/// Creating the grid to automatically fit the board panel
/// </summary>
{


    //Setting the grid size
    public int columns = 5;
    public int rows = 5;


    public RectTransform gridParent; //This is the board panel
    public GameObject tilePrefab;

                                
    public Vector2 spacing = new Vector2(20, 20);//This is making a small gap between the tiles on the grid so they're not clumped together

    //Make the tile size bigger
    public float tileMultiplier = 1f;

    private GameObject[,] grid; //the grid that will store the tiles
    private GridLayoutGroup gridLayout; //Refrencing the Unity Grid layout group componet that is on the board as a varible called gridLayout

    // Allows other scripts (like chests) to access the grid manager easily
    public static GridManager Instance;

    // Tracks whether each tile in the grid is occupied
    private bool[,] occupied;
    private void Awake()
    {
        // Set up singleton reference
        Instance = this;
    }

    private void Start()
    {
        gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        gridLayout.spacing = spacing;
        
        //Getting the
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns; //setting to 5 
        //Creating the tiles

        CalculateCellSize();
        GenerateGrid();
    }

    /// <summary>
    /// Calculating the width and height of each tile so the will all fit evenly in the board panel
    /// </summary>
    private void CalculateCellSize()
    {
        float totalWidth = gridParent.rect.width;
        Debug.Log("Total Width: " + totalWidth);
        float totalHeight = gridParent.rect.height;
        Debug.Log("Total Height: " + totalHeight);

        //Take the width of the board subtract the spacing inbetween and split the remaining width even between the tiles
        float cellWidth = (totalWidth - spacing.x * (columns - 1)) / columns;
        //Take the height of the board subtract the spacing inbetween then place the tiles evenly in the remaining height
        float cellHeight = (totalHeight - spacing.y * (rows - 1)) / rows;

        //Adding in the multiplier
        cellWidth *= tileMultiplier;
        cellHeight *= tileMultiplier;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }

    /// <summary>
    /// 
    /// </summary>
    private void GenerateGrid()
    {
        // Initialize occupancy tracking
        occupied = new bool[columns, rows];
        
        grid = new GameObject[columns, rows];


        for (int j = 0; j < rows; j++){
            for (int i = 0; i < columns; i++)
            {
                //Create a tile
                GameObject tile = Instantiate(tilePrefab, gridParent);
                tile.name = $"Tile ({i},{j})";//giving each tile a name so I can identify it in the hierarchy


                //Initializing the TileUI
                TileUI tileUI = tile.GetComponent<TileUI>();
                if (tileUI != null) 
                {
                    tileUI.Init(i, j);
                }


                //Storing the tile
                grid[i, j] = tile;

                // Mark tile as empty at the start
                occupied[i, j] = false;
            }
        }

    }

  

    /// <summary>
    /// Marks a tile as occupied after an item is placed
    /// </summary>
    public void OccupyTile(int x, int y)
    {
        occupied[x, y] = true;
    }

    /// <summary>
    /// Returns the transform of a tile at a given grid position
    /// </summary>
    public Transform GetTileTransform(int x, int y)
    {
        return grid[x, y].transform;
    }

    // ------------------------------------------------------------
    // Helper methods used by MergeBoardController
    // ------------------------------------------------------------

    /// <summary>
    /// Returns true if (x,y) is inside the grid
    /// </summary>


    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public TileUI GetTileUI(int x, int y)
    {
        if (!InBounds(x, y)) return null;

        GameObject tileObj = grid[x, y];
        if (tileObj == null) return null;

        return tileObj.GetComponent<TileUI>();
    }
    /// <summary>
    /// Returns a random empty tile position (where TileUI.currentItem == null).
    /// </summary>
    public bool TryGetRandomEmptyTile(out Vector2Int pos)
    {
        List<Vector2Int> empties = new List<Vector2Int>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                TileUI t = GetTileUI(x, y);

                // Only count tiles that exist AND have no current item
                if (t != null && t.currentItem == null)
                    empties.Add(new Vector2Int(x, y));
            }
        }

        if (empties.Count == 0)
        {
            pos = default;
            return false;
        }

        pos = empties[Random.Range(0, empties.Count)];
        return true;
    }



}
