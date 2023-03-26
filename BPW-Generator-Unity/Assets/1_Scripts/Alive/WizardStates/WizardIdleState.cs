using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardIdleState : EnemyBaseState
{
    public int TargetRange; //range from currentPos a targetPos can be chosen

    private Tile targetTile;

    public override void OnStart()
    {
        targetTile = GetRandomTargetTile();
    }

    public override void OnUpdate()
    {
        List<Tile> path = aStar.CalcPath(enemy.GetCurrentTile(), targetTile, dungeonGrid, walkableTiles);
        if (path != null && path.Count >= 1 && path.Count <= GiveUpLenght)
        {
            Vector2Int pathPos = path[0].GetPos();
            if (dungeonGrid.IsTileAvailible(pathPos.x, pathPos.y, dungeonGrid.walkableTiles))
            {
                enemy.SetPos(pathPos);
            }
        }
        else
        {
            targetTile = GetRandomTargetTile();
        }
    }

    private Tile GetRandomTargetTile()
    {
        Vector2Int enemyPos = enemy.GetPos();
        Vector2Int posOne = new Vector2Int(enemyPos.x - TargetRange, enemyPos.y - TargetRange);
        Vector2Int posTwo = new Vector2Int(enemyPos.x + TargetRange, enemyPos.y + TargetRange);
        return dungeonGrid.GetRandomTile(posOne.x, posOne.y, posTwo.x, posTwo.y, walkableTiles);
    }
}