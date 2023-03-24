using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGrid : IntGrid
{
    public int DarknessLevel; //Between 0 and 11

    public override void OnStart()
    {
        base.OnStart();

        FillWithDarkness();
        gridRenderer.Draw();
    }

    private void FillWithDarkness()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gridArray[x, y] = new IntTile(DarknessLevel, new Vector2Int(x, y));
            }
        }
    }
}
