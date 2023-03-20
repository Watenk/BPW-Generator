using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Alive
{
    private Vector2Int up = new Vector2Int(0, -1);
    private Vector2Int right = new Vector2Int(1, 0);
    private Vector2Int down = new Vector2Int(0, 1);
    private Vector2Int left = new Vector2Int(-1, 0);

    private InputManager inputManager;
    private DungeonGrid dungeonGrid;

    public override void OnAwake()
    {
        dungeonGrid = FindObjectOfType<DungeonGrid>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public override void OnUpdate()
    {
        Checkinputs();
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

    public void Move(Vector2Int direction)
    {
        Vector2Int currentPos = GetPos();
        Vector2Int newPos = new Vector2Int(currentPos.x + direction.x, currentPos.y + direction.y);
        if (dungeonGrid.IsTileAvailible(newPos.x, newPos.y, dungeonGrid.walkableTiles))
        {
            SetPos(newPos);
        }
    }
}
