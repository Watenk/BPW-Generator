using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : BaseClass
{
    public delegate void NextTurn();
    public static event NextTurn OnNextTurn;

    public delegate void HealthPotionUsed();
    public static event HealthPotionUsed OnHealthPotionUsed;


    [ContextMenu("TriggerNextTurn")]
    public void TriggerNextTurn()
    {
        OnNextTurn();
    }

    public void TriggerHealthPotionUsed()
    {
        OnHealthPotionUsed();
    }
}