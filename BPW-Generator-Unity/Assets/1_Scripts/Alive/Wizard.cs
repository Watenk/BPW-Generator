using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    public override void UpdateStates()
    {
        Vector2Int enemyPos = GetPos();
        Vector2Int playerPos = dungeonGrid.GetEntity(0).GetPos();

        int distance = Mathf.Abs(playerPos.x - enemyPos.x) + Mathf.Abs(playerPos.y - enemyPos.y);
        if (distance <= PlayerDetectRange && aStar.CalcPath(dungeonGrid.GetTile(GetPos().x, GetPos().y), dungeonGrid.GetEntity(0).GetCurrentTile(), dungeonGrid, dungeonGrid.walkableTiles) != null)
        {
            attackState.SwitchState(typeof(WizardAttackState));
        }
        else
        {
            attackState.SwitchState(typeof(WizardIdleState));
        }
    }
}