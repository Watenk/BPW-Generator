using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : BaseClass
{
    public int Width;
    public int Height;

    protected Tile[,] gridArray;

    public override void OnStart()
    {
        gridArray = new Tile[Width, Height];
        PrintGridSize();
    }

    public Tile GetTile(int x, int y) 
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return gridArray[x, y]; 
        }
        return null;
    }

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

    public bool IsTileAvailible(int x, int y)
    {
        Tile currentTile = GetTile(x, y);
        if (currentTile != null)
        {
            if (currentTile.GetID() == ID.none || currentTile.GetID() == ID.wall)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void PrintGridSize()
    {
        if (gridArray.Length < 1000000)
        {
            Debug.Log(gameObject.name + " size: " + gridArray.Length / 1000 + "K");
        }
        else
        {
            Debug.Log(gameObject.name + " size: " + gridArray.Length / 1000000 + "M");
        }
    }
}