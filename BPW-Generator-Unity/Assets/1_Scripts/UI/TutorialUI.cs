using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : BaseClass
{
    public GameObject movement;
    public GameObject goal;

    public override void OnStart()
    {
        goal.SetActive(false);
    }

    public void DisableMovement() 
    {
        movement.SetActive(false);
    }

    public void EnableGoal()
    {
        goal.SetActive(true);
    }

    public void DisableGoal()
    {
        goal.SetActive(false);
    }
}