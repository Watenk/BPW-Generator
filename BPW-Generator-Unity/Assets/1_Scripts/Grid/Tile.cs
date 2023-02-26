using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ID
{
    none,
    floor,
    wall,
}

public class Tile
{
    protected ID id;

    public Tile(ID id)
    {
        this.id = id;
    }

    public ID GetID() { return id; }
    public void SetID(ID id) { this.id = id; }
}