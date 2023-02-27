using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGrid : Grid
{
    public int MinRoomSize;
    public int MaxRoomSize;
    public int RoomAmount;
    public int RetryFailedRoomGeneration;
    public ID Wall;
    public ID Floor;

    private Room[] rooms;
    private WatenkLib watenkLib;

    public override void OnStart()
    {
        rooms = new Room[RoomAmount];
        watenkLib = new WatenkLib();
        base.OnStart();

        FillGridWithWall();
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        for (int i = 0; i < RoomAmount; i++)
        {
            int currentRetryFailedRoomGeneration = 0;

        startRoomGeneration:
            int xStartLocation = watenkLib.GetRandomInt(0, Width);
            int yStartLocation = watenkLib.GetRandomInt(0, Height);
            int xRoomSize = watenkLib.GetRandomInt(MinRoomSize, MaxRoomSize);
            int yRoomSize = watenkLib.GetRandomInt(MinRoomSize, MaxRoomSize);

            if (AreTilesAvailible(xStartLocation, yStartLocation, xRoomSize, yRoomSize))
            {
                //Tile[,] roomArray = new Tile[Width, Height];
                for (int y = yStartLocation; y < yStartLocation + yRoomSize; y++)
                {
                    for (int x = xStartLocation; x < xStartLocation + xRoomSize; x++)
                    {
                        SetTile(x, y, Floor);
                        //roomArray[x, y] = GetTile(x, y);
                    }
                }
                //rooms[i] = new Room(roomArray, Width, Height);
            }
            else
            {
                currentRetryFailedRoomGeneration++;
                if (currentRetryFailedRoomGeneration <= RetryFailedRoomGeneration)
                {
                    goto startRoomGeneration;
                }
                else
                {
                    Debug.Log("Room Generation Failed");
                }
            }
        }
    }

    private bool AreTilesAvailible(int xStartLocation, int yStartLocation, int xRoomSize, int yRoomSize)
    {
        for (int y = yStartLocation; y < yStartLocation + yRoomSize; y++)
        {
            for (int x = xStartLocation; x < xStartLocation + xRoomSize; x++)
            {
                if (IsTileAvailible(x, y) == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void FillGridWithWall()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                gridArray[x, y] = new Tile(Wall);
            }
        }
    }


}