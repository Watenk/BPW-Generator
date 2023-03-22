using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : BaseClass
{
    private Vector2Int pos;
    private int ID;

    protected static Vector2Int up = new Vector2Int(0, -1);
    protected static Vector2Int right = new Vector2Int(1, 0);
    protected static Vector2Int down = new Vector2Int(0, 1);
    protected static Vector2Int left = new Vector2Int(-1, 0);

    private DungeonGrid dungeonGrid;

    public override void OnAwake()
    {
        dungeonGrid = FindObjectOfType<DungeonGrid>();
    }

    public override void OnStart()
    {
        EventManager.OnNextTurn += OnNextTurn;
    }

    public Vector2Int GetPos() 
    { 
        return pos; 
    }

    public void SetPos(Vector2Int newPos) 
    {
        gameObject.transform.position = new Vector3(newPos.x, -newPos.y, 0f);
        pos = newPos;
    }

    public Tile GetCurrentTile()
    {
        return dungeonGrid.GetTile(pos.x, pos.y);
    }

    public int GetID() 
    { 
        return ID; 
    }

    public void SetID(int newID) 
    { 
        ID = newID; 
    }

    public void Move(Vector2Int direction)
    {
        Vector2Int currentPos = GetPos();
        Vector2Int newPos = new Vector2Int(currentPos.x + direction.x, currentPos.y + direction.y);
        if (dungeonGrid.IsTileAvailible(newPos.x, newPos.y, dungeonGrid.walkableTiles))
        {
            SetPos(newPos);
        }
    }

    public virtual void OnNextTurn()
    {

    }

}
