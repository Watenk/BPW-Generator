using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStar
{
    //A self-written attempt at AStar
    public List<Tile[]> CalcFastestPath(Tile startTile, Tile targetTile, Grid grid)
    {
        Dictionary<Tile, int> gCost = new Dictionary<Tile, int>(); //G-Cost is distance from start
        Dictionary<Tile, int> hCost = new Dictionary<Tile, int>(); ; //H-Cost is distance from target
        Dictionary<Tile, int> fCost = new Dictionary<Tile, int>(); //Tiles with calculated fCost
        List<Tile> calcedTiles = new List<Tile>(); //Tiles that are already calculated
        List<Tile> pendingTiles = new List<Tile>(); //Tiles pending to calc fCost
        List<Tile[]> path = new List<Tile[]>(); //List of tiles with fastest path



        return path;
    }

    private void CalcSurroundingTiles(Tile currentTile, Dictionary<Tile, int> fCost, Grid grid, List<Tile> calcedTiles)
    {
        Vector2Int currentTilePos = currentTile.GetPos();
        //up
        Tile upTile = grid.GetTile(currentTilePos.x, currentTilePos.y - 1);
        if (!calcedTiles.Contains(upTile))
        {
            //CalcTileCost();
        }

        //right
        Tile rightTile = grid.GetTile(currentTilePos.x + 1, currentTilePos.y);

        //down
        Tile downTile = grid.GetTile(currentTilePos.x, currentTilePos.y + 1);

        //left
        Tile leftTile = grid.GetTile(currentTilePos.x - 1, currentTilePos.y);

    }

    private void CalcTileCost(Tile currentTile, Tile startTile, Tile targetTile, Dictionary<Tile, int> gCost, Dictionary<Tile, int> hCost, Dictionary<Tile, int> fCost, List<Tile> calcedTiles)
    {
        Vector2Int currentTilePos = currentTile.GetPos();
        Vector2Int startTilePos = startTile.GetPos();
        Vector2Int targetTilePos = targetTile.GetPos();

        //CalcGCost
        int xDifferenceGCost = Mathf.Abs(currentTilePos.x - startTilePos.x);
        int yDifferenceGCost = Mathf.Abs(currentTilePos.y - startTilePos.y);
        int gCostInt = xDifferenceGCost + yDifferenceGCost;
        gCost.Add(currentTile, gCostInt);

        //CalcHCost
        int xDifferenceHCost = Mathf.Abs(currentTilePos.x - targetTilePos.x);
        int yDifferenceHCost = Mathf.Abs(currentTilePos.y - targetTilePos.y);
        int hCostInt = xDifferenceHCost + yDifferenceHCost;
        hCost.Add(currentTile, hCostInt);

        //CalcFCost
        int fCostInt = gCostInt + hCostInt;
        fCost.Add(currentTile, fCostInt);

        calcedTiles.Add(currentTile);
    }
}