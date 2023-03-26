using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Alive
{
    public int PlayerDetectRange;
    public FSM attackState;

    protected AStar aStar;

    public override void OnStart()
    {
        base.OnStart();
        aStar = new AStar();
        attackState = new FSM(GetComponents<BaseState>());
    }

    public override void OnNextTurn()
    {
        UpdateStates();
        attackState.OnUpdate();
    }

    public override void Die()
    {
        EventManager.OnNextTurn -= OnNextTurn;
        dungeonGrid.RemoveEntity(this.gameObject);
        base.Die();
    }

    public virtual void UpdateStates()
    {

    }
}