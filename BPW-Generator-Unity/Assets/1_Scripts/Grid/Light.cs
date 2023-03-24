using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : BaseClass
{
    public int LightRange;

    private Alive entity;

    public override void OnAwake()
    {
        entity = GetComponent<Enemy>();
    }

    public Vector2Int GetPos()
    {
        return entity.GetPos();
    }
}