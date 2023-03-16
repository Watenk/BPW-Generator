using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : BaseClass
{
    public float ScrollSpeed;
    public int minCamSize;
    public int maxCamSize;

    private InputManager inputManager;
    private Vector2 referenceMousePos;

    public override void OnAwake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    public override void OnUpdate()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (inputManager.MiddleMouseDown == true)
        {
            referenceMousePos = Input.mousePosition;
            referenceMousePos = Camera.main.ScreenToWorldPoint(referenceMousePos);
        }

        if (inputManager.MiddleMouse == true)
        {
            //Get mousepos and calc newPos
            Vector2 currentMousePos = Input.mousePosition;
            currentMousePos = Camera.main.ScreenToWorldPoint(currentMousePos);
            float xDifference = currentMousePos.x - referenceMousePos.x;
            float yDifference = currentMousePos.y - referenceMousePos.y;
            float newXPos = Camera.main.transform.position.x - xDifference;
            float newYPos = Camera.main.transform.position.y - yDifference;

            //Set newPos
            Vector3 newPos = new Vector3(newXPos, newYPos, -10);
            Camera.main.transform.position = newPos;
        }

        if (inputManager.ScrollMouseDelta > 0f && Camera.main.orthographicSize > minCamSize && Input.GetMouseButton(2) == false) //Scroll up
        {
            Camera.main.orthographicSize -= Camera.main.orthographicSize * ScrollSpeed * 0.01f;
        }

        if (inputManager.ScrollMouseDelta < 0f && Camera.main.orthographicSize < maxCamSize && Input.GetMouseButton(2) == false) //Scroll down
        {
            Camera.main.orthographicSize += Camera.main.orthographicSize * ScrollSpeed * 0.01f;
        }
    }
}