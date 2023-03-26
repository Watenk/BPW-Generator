using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : BaseClass
{
    public int ID;
    public GameObject sprite;
    public int Health;

    protected static Vector2Int up = new Vector2Int(0, -1);
    protected static Vector2Int right = new Vector2Int(1, 0);
    protected static Vector2Int down = new Vector2Int(0, 1);
    protected static Vector2Int left = new Vector2Int(-1, 0);

    private Vector2Int pos;

    protected DungeonGrid dungeonGrid;
    private GameManager gameManager;

    public override void OnAwake()
    {
        dungeonGrid = FindObjectOfType<DungeonGrid>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void OnStart()
    {
        EventManager.OnNextTurn += OnNextTurn;
    }

    //Getters----------------------------------------

    public Vector2Int GetPos() 
    {
        return pos; 
    }

    public int GetID()
    {
        return ID;
    }

    public int GetHealth()
    {
        return Health;
    }

    public Tile GetCurrentTile()
    {
        return dungeonGrid.GetTile(pos.x, pos.y);
    }

    //Setters-----------------------------------------

    public void SetPos(Vector2Int newPos) 
    {
        gameObject.transform.position = new Vector3(newPos.x, -newPos.y, 0f);
        pos = newPos;
    }

    public void SetID(int newID) 
    { 
        ID = newID; 
    }

    public void SetHealth(int value)
    {
        Health = value;
        CheckIfDeath();
    }

    public void AddHealth(int value)
    {
        Health += value;
        CheckIfDeath();
    }

    public void RemoveHealth(int value)
    {
        Health -= value;
        CheckIfDeath();
    }

    //Functions---------------------------------------

    public virtual void Move(Vector2Int direction)
    {
        Vector2Int currentPos = GetPos();
        Vector2Int newPos = new Vector2Int(currentPos.x + direction.x, currentPos.y + direction.y);

        if (dungeonGrid.IsTileAvailible(newPos.x, newPos.y, dungeonGrid.walkableTiles))
        {
            SetPos(newPos);
        }
    }

    public virtual void Die()
    {
        gameManager.RemoveObject(this.gameObject);
    }

    private void CheckIfDeath()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void OnNextTurn()
    {

    }
}
