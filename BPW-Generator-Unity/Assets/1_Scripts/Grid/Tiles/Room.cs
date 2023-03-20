using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private int xLenght;
    private int yLenght;
    private int x1;
    private int y1;
    private int x2;
    private int y2;

    public Room(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        xLenght = x2 - x1;
        yLenght = y2 - y1;
    }

    public Vector2Int GetMiddle()
    {
        int xMiddle = xLenght / 2;
        int yMiddle = yLenght / 2;
        return new Vector2Int(x1 + xMiddle, y1 + yMiddle);
    }

    public Vector2Int GetRandomPos() 
    {
        int xPos = x1 + Random.Range(0, xLenght);
        int yPos = y1 + Random.Range(0, yLenght);
        return new Vector2Int(xPos, yPos);
    }
}