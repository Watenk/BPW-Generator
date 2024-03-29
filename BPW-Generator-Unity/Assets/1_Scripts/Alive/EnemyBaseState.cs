using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : BaseState
{
    public int GiveUpLenght; //Give up if path is longer 
    public List<ID> walkableTiles = new List<ID>();
    public GameObject AttackParticle;

    protected AStar aStar;
    protected DungeonGrid dungeonGrid;
    protected Enemy enemy;

    public void Awake()
    {
        aStar = new AStar();
        dungeonGrid = FindObjectOfType<DungeonGrid>();
        enemy = gameObject.GetComponent<Enemy>();
    }
    public virtual void AttackPlayer(int damage)
    {
        dungeonGrid.RemoveHealth(0, damage);
        Instantiate(AttackParticle, new Vector3(enemy.GetPos().x, -enemy.GetPos().y, -2), Quaternion.identity);
    }
}