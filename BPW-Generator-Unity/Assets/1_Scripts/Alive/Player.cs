using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Alive
{
    public int MovesPerTurn; //Amount of moves the player can do before ending the turn
    public int MoveCost; //amount of moves subtracts after moving a tile
    public int ReplaceTileCost; //Amount of moves subtracts after replacing a tile
    public int remainingMoves;
    public int Damage; //Amount of damage player does
    public GameObject AttackParticle;

    private bool inputsLocked;

    private InputManager inputManager;
    private EventManager eventManager;
    private LightGrid lightGrid;
    private UI ui;

    public override void OnAwake()
    {
        base.OnAwake();
        inputManager = FindObjectOfType<InputManager>();
        eventManager = FindObjectOfType<EventManager>();
        lightGrid = FindObjectOfType<LightGrid>();
        ui = FindObjectOfType<UI>();
    }

    public override void OnStart()
    {
        base.OnStart();
        remainingMoves = MovesPerTurn;
        lightGrid.AddLight(GetPos());
    }

    public override void OnUpdate()
    {
        if (!inputsLocked) { Checkinputs(); }
    }

    private void Checkinputs()
    {
        if (inputManager.W == true)
        {
            Move(up);
        }

        if (inputManager.D == true)
        {
            Move(right);
        }

        if (inputManager.S == true)
        {
            Move(down);
        }

        if (inputManager.A == true)
        {
            Move(left);
        }
    }

    public override void Move(Vector2Int direction)
    {
        //Move a Tile
        Vector2Int currentPos = GetPos();
        Vector2Int newPos = new Vector2Int(currentPos.x + direction.x, currentPos.y + direction.y);
        if (dungeonGrid.IsTileAvailible(newPos.x, newPos.y, dungeonGrid.walkableTiles))
        {
            lightGrid.MoveLight(GetPos(), direction);
            lightGrid.UpdateLights();
            SetPos(newPos);
            AddMove(MoveCost);
        }
        //Replace a tile
        else if (dungeonGrid.IsInGridBounds(newPos.x, newPos.y) && dungeonGrid.GetEntity(newPos) == null)
        {
            dungeonGrid.SetTile(newPos.x, newPos.y, global::ID.pavedStone, true);
            AddMove(ReplaceTileCost);
        }
        //Attack Enemy
        else if (dungeonGrid.GetEntity(newPos) != null)
        {
            AttackEnemy(dungeonGrid.GetEntity(newPos));
        }
    }

    public override void RemoveHealth(int value)
    {
        base.RemoveHealth(value);
        ui.UpdatePlayerHealth(Health);
    }

    private void AttackEnemy(Alive enemy)
    {
        enemy.RemoveHealth(Damage);
        Instantiate(AttackParticle, new Vector3(GetPos().x, -GetPos().y, -2), Quaternion.identity);
        AddMove(1);
    }

    private bool CheckRemainingMoves()
    {
        if (remainingMoves <= 0)
        {
            return false;
        }
        return true;
    }

    private void AddMove(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            remainingMoves -= 1;
            NextTurnCheck();
        }
    }

    private void NextTurnCheck()
    {
        if (!CheckRemainingMoves())
        {
            eventManager.TriggerNextTurn();
            remainingMoves = MovesPerTurn;
        }
    }
}