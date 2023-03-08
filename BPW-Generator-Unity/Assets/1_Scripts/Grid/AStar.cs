using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStar
{
    //A self-written attempt at AStar
    public List<Tile> CalcFastestPath(Tile startTile, Tile targetTile, Grid grid)
    {
        Dictionary<Tile, int> gCost = new Dictionary<Tile, int>(); //G-Cost is distance from start
        Dictionary<Tile, int> hCost = new Dictionary<Tile, int>(); ; //H-Cost is distance from target
        Dictionary<Tile, int> fCost = new Dictionary<Tile, int>(); //Tiles with calculated fCost
        Dictionary<Tile, Tile> parent = new Dictionary<Tile, Tile>(); //Parent of tile
        List<Tile> calcedTiles = new List<Tile>(); //Already calculated tiles
        List<Tile> pendingTiles = new List<Tile>(); //Tiles to explore
        List<Tile> path = new List<Tile>(); //List of tiles with fastest path

        Tile currentTile = startTile;

        //Calc all low fCosts until targetTile
        while (currentTile != targetTile)
        {
            CalcSurroundingTiles(currentTile, startTile, targetTile, gCost, hCost, fCost, grid, parent, calcedTiles);
            currentTile = GetLowestFCost(fCost);
        }

        //Retrace fastest path from targetTile


        return path;
    }

    private Tile GetLowestFCost(Dictionary<Tile, int> fCost)
    {
        return fCost.Aggregate((x, y) => x.Value < y.Value ? x : y).Key; // x and y are the key and the value from the dictionary
    }

    private void CalcSurroundingTiles(Tile currentTile, Tile startTile, Tile targetTile, Dictionary<Tile, int> gCost, Dictionary<Tile, int> hCost, Dictionary<Tile, int> fCost, Grid grid, Dictionary<Tile, Tile> parent, List<Tile> calcedTiles)
    {
        Vector2Int currentTilePos = currentTile.GetPos();
        //up
        Tile upTile = grid.GetTile(currentTilePos.x, currentTilePos.y - 1);
        CalcTileCost(upTile, startTile, targetTile, currentTile, gCost, hCost, fCost, parent, calcedTiles);

        //right
        Tile rightTile = grid.GetTile(currentTilePos.x + 1, currentTilePos.y);
        CalcTileCost(rightTile, startTile, targetTile, currentTile, gCost, hCost, fCost, parent, calcedTiles);

        //down
        Tile downTile = grid.GetTile(currentTilePos.x, currentTilePos.y + 1);
        CalcTileCost(downTile, startTile, targetTile, currentTile, gCost, hCost, fCost, parent, calcedTiles);

        //left
        Tile leftTile = grid.GetTile(currentTilePos.x - 1, currentTilePos.y);
        CalcTileCost(leftTile, startTile, targetTile, currentTile, gCost, hCost, fCost, parent, calcedTiles);
    }

    private void CalcTileCost(Tile currentTile, Tile startTile, Tile targetTile, Tile parentTile, Dictionary<Tile, int> gCost, Dictionary<Tile, int> hCost, Dictionary<Tile, int> fCost, Dictionary<Tile, Tile> parent, List<Tile> calcedTiles)
    {
        if (!calcedTiles.Contains(currentTile))
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

            parent.Add(currentTile, parentTile);
        }
    }
}