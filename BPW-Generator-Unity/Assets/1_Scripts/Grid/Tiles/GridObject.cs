using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectID
{
    none,
    crystal,
}

public class GridObject : BaseClass
{
    public GameObject sprite;

    private Vector2Int pos;
    private int id;
    private objectID objectID;

    private GameManager gameManager;

    public override void OnAwake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void PickUp()
    {
        gameManager.RemoveObject(this.gameObject);
    }

    public Vector2Int GetPos()
    {
        return pos;
    }

    public int GetId() 
    {
        return id;
    }

    public objectID GetObjectID()
    {
        return objectID;
    }

    public void SetPos(Vector2Int newPos)
    {
        gameObject.transform.position = new Vector3(newPos.x, -newPos.y, 0f);
        this.pos = newPos;
    }

    public void SetId(int id) 
    {
        this.id = id;
    }

    public void SetObjectID(objectID objectID) 
    {
        this.objectID = objectID;
    }
}