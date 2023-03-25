using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Alive
{
    public int PlayerDetectRange;
    public FSM attackState;

    public override void OnStart()
    {
        base.OnStart();
        attackState = new FSM(GetComponents<BaseState>());
        attackState.SwitchState(typeof(EnemyIdleState));
    }

    public override void OnNextTurn()
    {
        UpdateState();
        attackState.OnUpdate();
    }

    private void UpdateState()
    {
        Vector2Int enemyPos = GetPos();
        Vector2Int playerPos = dungeonGrid.GetEntity(0).GetPos();

        int distance = Mathf.Abs(playerPos.x - enemyPos.x) + Mathf.Abs(playerPos.y - enemyPos.y);
        if (distance <= PlayerDetectRange)
        {
            attackState.SwitchState(typeof(EnemyAttackState));
        }
        else
        {
            attackState.SwitchState(typeof(EnemyIdleState));
        }
    }
}