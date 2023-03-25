using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Alive
{
    public int MovesPerTurn; //Amount of moves the player can do before ending the turn
    public int MoveCost; //amount of moves subtracts after moving a tile
    public int ReplaceTileCost; //Amount of moves subtracts after replacing a tile
    public int remainingMoves;

    private bool inputsLocked;

    private InputManager inputManager;
    private EventManager eventManager;
    private LightGrid lightGrid;

    public override void OnAwake()
    {
        base.OnAwake();
        inputManager = FindObjectOfType<InputManager>();
        eventManager = FindObjectOfType<EventManager>();
        lightGrid = FindObjectOfType<LightGrid>();
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
        else if (dungeonGrid.IsInGridBounds(newPos.x, newPos.y))
        {
            dungeonGrid.SetTile(newPos.x, newPos.y, global::ID.pavedStone, true);
            AddMove(ReplaceTileCost);
        }
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