using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : BaseClass
{
    private Vector2Int pos;

    public Vector2Int GetPos() { return pos; }
    public void SetPos(Vector2Int newPos) 
    {
        gameObject.transform.position = new Vector3(newPos.x, -newPos.y, 0f);
        pos = newPos;
    }
}
