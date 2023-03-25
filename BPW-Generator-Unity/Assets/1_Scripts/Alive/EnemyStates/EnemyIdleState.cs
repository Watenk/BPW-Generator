using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
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
            enemy.SetPos(path[0].GetPos());
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
