using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntGrid : BaseClassLate
{
    public IntGridRenderer gridRenderer;
    protected IntTile[,] gridArray;

    [HideInInspector]
    public int width;
    [HideInInspector]
    public int height;

    private DungeonGrid dungeonGrid;

    public override void OnAwake()
    {
        dungeonGrid = FindObjectOfType<DungeonGrid>();
    }

    public override void OnStart()
    {
        width = dungeonGrid.Width;
        height = dungeonGrid.Height;
        gridArray = new IntTile[dungeonGrid.Width, dungeonGrid.Height];
        PrintGridSize();
    }

    public IntTile GetTile(int x, int y)
    {
        if (IsInGridBounds(x, y))
        {
            return gridArray[x, y];
        }
        return null;
    }

    public void SetTile(int x, int y, int value, bool redrawGrid)
    {
        if (IsInGridBounds(x, y))
        {
            IntTile currentTile = GetTile(x, y);
            if (currentTile.GetValue() != value)
            {
                currentTile.SetValue(value);
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

    public void SetTiles(int x1, int y1, int x2, int y2, int value, bool redrawGrid)
    {
        for (int y = y1; y < y2; y++)
        {
            for (int x = x1; x < x2; x++)
            {
                SetTile(x, y, value, redrawGrid);
            }
        }
    }

    public bool IsInGridBounds(int x, int y)
    {
        if (x >= 0 && x <= width && y >= 0 && y <= height)
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