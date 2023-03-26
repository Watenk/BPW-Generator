using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyBaseState
{
    public int Damage;

    public override void OnUpdate()
    {
        base.OnUpdate();
        Tile playerPos = dungeonGrid.GetEntity(0).GetCurrentTile();
        List<Tile> path = aStar.CalcPath(enemy.GetCurrentTile(), playerPos, dungeonGrid, walkableTiles);

        if (path != null && path.Count >= 1 && path.Count <= GiveUpLenght)
        {
            Vector2Int pathPos = path[0].GetPos();
            if (dungeonGrid.IsTileAvailible(pathPos.x, pathPos.y, dungeonGrid.walkableTiles))
            {
                enemy.SetPos(pathPos);
            }
            else
            {
                if (dungeonGrid.GetEntity(0).GetPos() == path[0].GetPos()) //Attack player
                {
                    AttackPlayer(Damage);
                }
            }
        }
    }
}