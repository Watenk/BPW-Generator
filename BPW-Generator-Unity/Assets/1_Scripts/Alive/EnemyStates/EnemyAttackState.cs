using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        Tile playerPos = dungeonGrid.GetEntity(0).GetCurrentTile();
        List<Tile> path = aStar.CalcPath(enemy.GetCurrentTile(), playerPos, dungeonGrid, walkableTiles);

        if (path != null && path.Count >= 1 && path.Count <= GiveUpLenght)
        {
            enemy.SetPos(path[0].GetPos());
        }
        else
        {
            owner.SwitchState(typeof(EnemyIdleState));
        }
    }
}