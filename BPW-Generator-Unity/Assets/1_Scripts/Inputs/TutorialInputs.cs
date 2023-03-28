using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInputs : BaseClass
{
    public int TimerAmount;

    private TutorialUI tutorialUI;
    private InputManager inputManager;

    //Movement
    private bool movementDone;
    private bool goalDone;
    private bool goal;

    public override void OnAwake()
    {
        tutorialUI = FindObjectOfType<TutorialUI>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public override void OnUpdate()
    {
        if (inputManager.W || inputManager.D || inputManager.S || inputManager.A || inputManager.F)
        {
            if (movementDone)
            {
                goalDone = true;
            }

            tutorialUI.DisableMovement();
            movementDone = true;
        }

        if (movementDone && !goal)
        {
            tutorialUI.EnableGoal();
            
            if (goalDone)
            {
                goal = true;
                tutorialUI.DisableGoal();
            }
        }
    }
}