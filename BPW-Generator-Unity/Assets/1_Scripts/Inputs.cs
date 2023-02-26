using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : BaseClass
{
    public Grid Grid;
    public float ScrollSpeed;

    private Vector2 referenceMousePos;
    private WatenkLib watenkLib;

    public override void OnAwake()
    {
        watenkLib = new WatenkLib();
    }

    public override void OnUpdate()
    {
        //Mouse ---------------------------------------------------------
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            watenkLib.ConvertMouseToInts(mousePos, out int mouseXInt, out int mouseYInt);
            Grid.SetTile(mouseXInt, -mouseYInt, ID.floor);
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            watenkLib.ConvertMouseToInts(mousePos, out int mouseXInt, out int mouseYInt);
            Grid.SetTile(mouseXInt, -mouseYInt, ID.wall);
        }

        if (Input.GetMouseButtonDown(2))
        {
            referenceMousePos = Input.mousePosition;
            referenceMousePos = Camera.main.ScreenToWorldPoint(referenceMousePos);
        }

        if (Input.GetMouseButton(2))
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

        if (Input.mouseScrollDelta.y > 0f && Camera.main.orthographicSize > 1 && Input.GetMouseButton(2) == false) //Scroll up
        {
            Camera.main.orthographicSize -= Camera.main.orthographicSize * ScrollSpeed * 0.01f;
        }

        if (Input.mouseScrollDelta.y < 0f && Input.GetMouseButton(2) == false) //Scroll down
        {
            Camera.main.orthographicSize += Camera.main.orthographicSize * ScrollSpeed * 0.01f;
        }
    }
}