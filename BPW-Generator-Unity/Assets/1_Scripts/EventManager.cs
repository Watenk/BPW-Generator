using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : BaseClass
{
    public delegate void NextTurn();
    public static event NextTurn OnNextTurn;

    public void TriggerNextTurn()
    {
        OnNextTurn();
    }
}