using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Alive
{
    public int MovesPerTurn; //Amount of moves the player can do before ending the turn
    public int remainingMoves;

    private bool inputsLocked;

    private InputManager inputManager;
    private EventManager eventManager;

    public override void OnAwake()
    {
        base.OnAwake();
        inputManager = FindObjectOfType<InputManager>();
        eventManager = FindObjectOfType<EventManager>();
    }

    public override void OnStart()
    {
        base.OnStart();
        remainingMoves = MovesPerTurn;
    }

    public override void OnUpdate()
    {
        if (!inputsLocked) { Checkinputs(); }
    }

    private void NextTurnCheck()
    {
        if (!CheckRemainingMoves())
        {
            eventManager.TriggerNextTurn();
            remainingMoves = MovesPerTurn;
        }
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
        base.Move(direction);
        remainingMoves -= 1;
        NextTurnCheck();
    }

    private bool CheckRemainingMoves()
    {
        if (remainingMoves <= 0)
        {
            return false;
        }
        return true;
    }
}