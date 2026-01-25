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
        float totalHeight = gridParent.rect.height;

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
            }
        }

    }


    
}
