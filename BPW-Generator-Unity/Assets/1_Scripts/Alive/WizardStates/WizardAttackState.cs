using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttackState : EnemyBaseState
{
    public int Damage;
    public int ShootRange;
    public int ShootChance; //Chance in percentages if a wizard will shoot at the player

    public override void OnUpdate()
    {
        base.OnUpdate();
        Tile playerPos = dungeonGrid.GetEntity(0).GetCurrentTile();
        List<Tile> path = aStar.CalcPath(enemy.GetCurrentTile(), playerPos, dungeonGrid, walkableTiles);

        if (path != null && path.Count >= 1 && path.Count <= GiveUpLenght)
        {
            Vector2Int pathPos = path[0].GetPos();

            for (int y = pathPos.y - ShootRange; y < pathPos.y + ShootRange; y++)
            {
                for (int x = pathPos.x - ShootRange; x < pathPos.x + ShootRange; x++)
                {
                    if (dungeonGrid.IsInGridBounds(x, y))
                    {
                        if (dungeonGrid.GetEntity(pathPos).GetID() == 0)
                        {
                            if (Random.Range(1, 100) < ShootChance)
                            {
                                AttackPlayer(Damage);
                            }
                        }
                    }
                }
            }

            if (dungeonGrid.IsTileAvailible(pathPos.x, pathPos.y, dungeonGrid.walkableTiles))
            {
                enemy.SetPos(pathPos);
            }
        }
    }
}