using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGrid : IntGrid
{
    public int LightRange; //Amount of tiles light will travel from a lightSource
    public List<ID> lightAllowedTiles = new List<ID>(); //Tiles light is allowed on - W.I.P (probably too much work)

    private int lightLevelAmount;
    private List<Vector2Int> lights = new List<Vector2Int>();

    public override void OnStart()
    {
        base.OnStart();
        lightLevelAmount = gridRenderer.TileAmount - 1;

        UpdateLights();
    }

    public void UpdateLights()
    {
        FillWithDarkness();
        for (int i = 0; i < lights.Count; i++)
        {
            int xPos = lights[i].x;
            int yPos = lights[i].y;

            for (int y = yPos - LightRange; y < yPos + LightRange; y++)
            {
                for (int x = xPos - LightRange; x < xPos + LightRange; x++)
                {
                    IntTile currentTile = GetTile(x, y);
                    if (currentTile != null)
                    {
                        int distance = Mathf.Abs(xPos - x) + Mathf.Abs(yPos - y);
                        if (distance <= LightRange)// && lightAllowedTiles.Contains(dungeonGrid.GetTile(x, y).GetID())) 
                        {
                            int value = Mathf.RoundToInt((float)lightLevelAmount / ((float)LightRange / (float)distance));
                            int difference = lightLevelAmount - currentTile.GetValue();
                            int newValue = value - difference;
                            if (newValue < lightLevelAmount)
                            {
                                newValue += Random.Range(-1, 2);
                            }
                            if (newValue < 0) { newValue = 0; }

                            SetTile(x, y, newValue, false);
                        }
                    }
                }
            }
        }
        CullEntitys();
        gridRenderer.Draw();
    }

    public void MoveLight(Vector2Int pos, Vector2Int direction)
    {
        if (lights.Contains(pos))
        {
            RemoveLight(pos);
            AddLight(pos + direction);
        }
    }

    public void AddLight(Vector2Int pos)
    {
        lights.Add(pos);
    }

    public void RemoveLight(Vector2Int pos)
    {
        if (lights.Contains(pos))
        {
            lights.Remove(pos);
        }
    }

    private void CullEntitys()
    {
        List<Alive> entitys = dungeonGrid.GetEntitys();
        for (int i = 0; i < entitys.Count; i++)
        {
            Vector2Int entityPos = entitys[i].GetPos();
            if (GetTile(entityPos.x, entityPos.y).GetValue() == lightLevelAmount)
            {
                dungeonGrid.SetSpriteActive(entitys[i].GetID(), false);
            }
            else
            {
                dungeonGrid.SetSpriteActive(entitys[i].GetID(), true);
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
