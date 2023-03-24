using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : BaseState
{
    public int TargetRange; //range from currentPos a targetPos can be chosen
    public List<ID> walkableTiles = new List<ID>();

    private AStar aStar;
    private DungeonGrid dungeonGrid;
    private Enemy enemy;

    private Tile targetTile;

    public void Awake()
    {
        aStar = new AStar();
        dungeonGrid = FindObjectOfType<DungeonGrid>();
        enemy = gameObject.GetComponent<Enemy>();
    }

    public override void OnStart()
    {
        targetTile = GetTargetTile();
    }

    public override void OnUpdate()
    {
        List<Tile> path = aStar.CalcPath(enemy.GetCurrentTile(), targetTile, dungeonGrid, walkableTiles); //Something wrong??
        if (path != null)
        {
            if (path.Count >= 1)
            {
                enemy.SetPos(path[0].GetPos());
            }
            else
            {
                targetTile = GetTargetTile();
            }
        }
        else
        {
            targetTile = GetTargetTile();
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
