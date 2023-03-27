using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseClass
{
    public bool W;
    public bool D;
    public bool S;
    public bool A;
    public bool F;
    public bool LeftMouse;
    public bool RightMouse;
    public bool MiddleMouse;
    public bool LeftMouseDown;
    public bool RightMouseDown;
    public bool MiddleMouseDown;
    public float ScrollMouseDelta;
    public float VerticalAxis;
    public float HorizontalAxis;

    void Update()
    {
        W = Input.GetKeyDown(KeyCode.W);
        D = Input.GetKeyDown(KeyCode.D);
        S = Input.GetKeyDown(KeyCode.S);
        A = Input.GetKeyDown(KeyCode.A);
        F = Input.GetKeyDown(KeyCode.F);
        LeftMouse = Input.GetKey(KeyCode.Mouse0);
        RightMouse = Input.GetKey(KeyCode.Mouse1);
        MiddleMouse = Input.GetKey(KeyCode.Mouse2);
        LeftMouseDown = Input.GetKeyDown(KeyCode.Mouse0);
        RightMouseDown = Input.GetKeyDown(KeyCode.Mouse1);
        MiddleMouseDown = Input.GetKeyDown(KeyCode.Mouse2);
        ScrollMouseDelta = Input.mouseScrollDelta.y;
        VerticalAxis = Input.GetAxis("Vertical");
        HorizontalAxis = Input.GetAxis("Horizontal");
    }
}