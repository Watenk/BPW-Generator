using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : BaseState
{
    private AStar aStar;

    public override void OnAwake()
    {
        aStar = new AStar();
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        //aStar.CalcPath();
    }

    public override void OnExit()
    {

    }
}
