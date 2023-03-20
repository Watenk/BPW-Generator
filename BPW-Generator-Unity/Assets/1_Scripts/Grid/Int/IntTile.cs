using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntTile
{
    protected int value;
    protected Vector2Int pos;

    public IntTile(int value, Vector2Int pos)
    {
        this.value = value;
        this.pos = pos;
    }

    public int GetValue() { return value; }
    public void SetValue(int value) { this.value = value; }
    public Vector2Int GetPos() { return pos; }
}