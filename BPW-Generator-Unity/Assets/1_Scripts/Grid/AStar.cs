using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public List<Vector2Int[]> CalcFastestPath(Tile[,] gridArray)
    {
        List<int[,]> gCost = new List<int[,]>(); //G-Cost is distance from start
        List<int[,]> hCost = new List<int[,]>(); //H-Cost is distance from target
        List<int[,]> totalCost = new List<int[,]>(); //Combo of H and G cost

        List<Vector2Int[]> path = new List<Vector2Int[]>();
        //Test if 
        return path;
    }
}