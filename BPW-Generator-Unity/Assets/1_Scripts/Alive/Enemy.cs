using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Alive
{
    public FSM attackState;

    public override void OnStart()
    {
        base.OnStart();
        attackState = new FSM(GetComponents<BaseState>());
        attackState.SwitchState(typeof(EnemyIdleState));
    }

    public override void OnNextTurn()
    {
        attackState.OnUpdate();
    }
}