using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Grid : BaseClass
{
    public int Width;
    public int Height;
    public ID WallTile;
    public ID FloorTile;

    private Tile[,] gridArray;

    public override void OnStart()
    {
        gridArray = new Tile[Width, Height];
        GenerateGrid();
        PrintGridSize();
    }

    public Tile GetTile(int x, int y) { return gridArray[x, y]; }

    public void SetTile(int x, int y, ID id) 
    { 
        if (x >= 0 && x <= Width && y >= 0 && y <= Height)
        {
            Tile currentTile = GetTile(x, y);
            currentTile.SetID(id);
        }
        else
        {
            Debug.Log("SetTile Out of Bounds: " + x + ", " + y);
        }
    }

    public void SetTiles(int x1, int y1, int x2, int y2, ID id)
    {
        for (int y = y1; y < y2; y++)
        {
            for (int x = x1; x < x2; x++)
            {
                SetTile(x, y, id);
            }
        }
    }

    private void GenerateGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                gridArray[x, y] = new Tile(ID.none);
            }
        }

        SetTiles(0, Height - 20, Width, Height, FloorTile); //Floor
        SetTiles(0, 0, 1, Height, WallTile); //Left wall
        SetTiles(Width - 1, 0, Width, Height, WallTile); //Right wall
        SetTiles(0, 0, Width, 1, WallTile); //Ceiling

        SetTiles(0, 0, Width, Height, FloorTile); // Fill Map
    }

    private void PrintGridSize()
    {
        if (gridArray.Length < 1000000)
        {
            Debug.Log("Grid size: " + gridArray.Length / 1000 + "K");
        }
        else
        {
            Debug.Log("Grid size: " + gridArray.Length / 1000000 + "M");
        }
    }
}