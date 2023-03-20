using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGrid : IntGrid
{
    public override void OnStart()
    {
        base.OnStart();

        FillWithDarkness();
    }

    private void FillWithDarkness()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gridArray[x, y] = new IntTile(0, new Vector2Int(x, y));
            }
        }
    }
}
