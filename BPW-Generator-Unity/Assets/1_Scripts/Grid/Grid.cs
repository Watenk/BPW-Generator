using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : BaseClassLate
{
    public GridRenderer gridRenderer;
    public int Width; //Wouldn't recommend anything above 500
    public int Height;

    protected Tile[,] gridArray;

    public override void OnStart()
    {
        gridArray = new Tile[Width, Height];
        PrintGridSize();
    }

    public Tile GetTile(int x, int y) 
    {
        if (IsInGridBounds(x, y))
        {
            return gridArray[x, y]; 
        }
        return null;
    }

    public void SetTile(int x, int y, ID id, bool redrawGrid) 
    { 
        if (IsInGridBounds(x, y))
        {
            Tile currentTile = GetTile(x, y);
            if (currentTile.GetID() != id)
            {
                currentTile.SetID(id);
                if (redrawGrid == true)
                {
                    gridRenderer.Draw();
                }
            }
        }
        else
        {
            Debug.Log("SetTile Out of Bounds: " + x + ", " + y);
        }
    }

    public void SetTiles(int x1, int y1, int x2, int y2, ID id, bool redrawGrid)
    {
        for (int y = y1; y < y2; y++)
        {
            for (int x = x1; x < x2; x++)
            {
                SetTile(x, y, id, redrawGrid);
            }
        }
    }

    public bool IsTileAvailible(int x, int y, ID[] allowedIDs)
    {
        if (GetTile(x, y) == null) { return false; }

        ID currentTileID = GetTile(x, y).GetID();
        for (int i = 0; i < allowedIDs.Length; i++)
        {
            if (currentTileID == allowedIDs[i])
            {
                return true;
            }
        }

        return false;
    }

    public bool AreTilesAvailible(int x1, int y1, int x2, int y2, ID[] allowedIDs)
    {
        if (IsInGridBounds(x1, y1) && IsInGridBounds(x2, y2))
        {
            for (int y = y1; y < y2; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    if (IsTileAvailible(x, y, allowedIDs) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }

    public bool IsInGridBounds(int x, int y)
    {
        if (x >= 0 && x <= Width && y >= 0 && y <= Height)
        {
            return true;
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