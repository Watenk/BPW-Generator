using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : BaseState
{
    public int TargetRange; //range from currentPos a targetPos can be chosen
    public List<ID> walkableTiles = new List<ID>();

    private AStar aStar;
    private Enemy enemy;
    private DungeonGrid dungeonGrid;

    private Tile targetTile;

    public override void OnAwake()
    {
        aStar = new AStar();
        enemy = gameObject.GetComponent<Enemy>();
        dungeonGrid = FindObjectOfType<DungeonGrid>();
    }

    public override void OnStart()
    {
        targetTile = GetTargetTile();
    }

    public override void OnUpdate()
    {
        List<Tile> path = aStar.CalcPath(enemy.GetCurrentTile(), targetTile, dungeonGrid, walkableTiles);
        if (path != null)
        {
            enemy.SetPos(path[0].GetPos());
        }
    }

    public override void OnExit()
    {

    }

    private Tile GetTargetTile()
    {
        Vector2Int enemyPos = enemy.GetPos();
        Vector2Int posOne = new Vector2Int(enemyPos.x - TargetRange, enemyPos.y - TargetRange);
        Vector2Int posTwo = new Vector2Int(enemyPos.x + TargetRange, enemyPos.y + TargetRange);
        return dungeonGrid.GetRandomTile(posOne.x, posOne.y, posTwo.x, posTwo.y, walkableTiles);
    }
}
