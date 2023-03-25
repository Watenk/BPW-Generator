using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGrid : IntGrid
{
    public int LightRange; //Range a light has

    private int lightLevelAmount;

    public override void OnStart()
    {
        base.OnStart();
        lightLevelAmount = gridRenderer.TileAmount - 1;

        FillWithDarkness();
        gridRenderer.Draw();
    }

    public void DrawLight(Vector2Int pos)
    {
        for (int y = pos.y - LightRange - 1; y < pos.y + LightRange + 1; y++)
        {
            for (int x = pos.x - LightRange - 1; x < pos.x + LightRange + 1; x++)
            {
                int distance = Mathf.Abs(pos.x - x) + Mathf.Abs(pos.y - y);
                int value = Mathf.RoundToInt((float)lightLevelAmount / ((float)LightRange / (float)distance));
                IntTile currentTileValue = GetTile(x, y);
                
                if (currentTileValue != null)
                {
                    if (distance <= LightRange)
                    {
                        SetTile(x, y, value, false);
                    }
                }
            }
        }
    }

    private void FillWithDarkness()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gridArray[x, y] = new IntTile(lightLevelAmount, new Vector2Int(x, y));
            }
        }
    }
}
