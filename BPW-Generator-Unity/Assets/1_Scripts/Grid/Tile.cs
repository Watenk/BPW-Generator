using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ID
{
    none,
    water,
    grass,
    stone,
    snow,
    pavedStone,
}

public class Tile
{
    protected ID id;
    protected Vector2Int pos;

    public Tile(ID id, Vector2Int pos)
    {
        this.id = id;
        this.pos = pos;
    }

    public ID GetID() { return id; }
    public void SetID(ID id) { this.id = id; }
    public Vector2Int GetPos() {  return pos; }
}