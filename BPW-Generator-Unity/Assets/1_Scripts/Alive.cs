using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : BaseClass
{
    private Vector2Int pos;

    public Vector2Int GetPos() { return pos; }
    public void SetPos(Vector2Int newPos) { pos = newPos; }
}
