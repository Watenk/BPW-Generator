using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Alive
{
    private int movesLeft;
    private bool inputsLocked;

    private InputManager inputManager;
    private EventManager eventManager;

    public override void OnAwake()
    {
        base.OnAwake();
        inputManager = FindObjectOfType<InputManager>();
        eventManager = FindObjectOfType<EventManager>();
    }

    public override void OnUpdate()
    {
        if (!inputsLocked) { Checkinputs(); }
    }

    private void NextMove()
    {

        if (movesLeft <= 0)
        {
            eventManager.TriggerNextTurn();
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


}
