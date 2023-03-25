using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : BaseState
{
    public int TargetRange; //range from currentPos a targetPos can be chosen
    public int GiveUpLenght; //Give up if path is longer 
    public List<ID> walkableTiles = new List<ID>();

    protected AStar aStar;
    protected DungeonGrid dungeonGrid;
    protected Enemy enemy;

    public void Awake()
    {
        aStar = new AStar();
        dungeonGrid = FindObjectOfType<DungeonGrid>();
        enemy = gameObject.GetComponent<Enemy>();
    }
}