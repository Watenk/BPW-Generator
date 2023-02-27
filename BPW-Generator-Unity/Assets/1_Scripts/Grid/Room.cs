using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Tile[,] roomList;

    public Room(Tile[,] roomList, int width, int height)
    {
        roomList = new Tile[width, height];
        this.roomList = roomList;
    }
}