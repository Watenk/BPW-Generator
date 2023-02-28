using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGrid : Grid
{
    //Terrain
    public ID[] WorldGenerationTiles = new ID[1]; //What tiles are used for generation
    public float PerlinMagnification; //The higher the bigger the tileCollections
    //Rooms
    public ID[] RoomTiles = new ID[1]; //Tiles where rooms can generate on
    public ID RoomsFloor; 
    public int RoomAmount;
    public int MinRoomSize;
    public int MaxRoomSize;
    public int RoomAvoidEdges; //Amount of tiles room will not generate from edge RoomTiles
    public int RetryFailedRoomGeneration; //Amount of times a room will trt to regenerate

    private float PerlinXOffset;
    private float PerlinYOffset;
    private Room[] rooms; //List of rooms in map

    private WatenkLib watenkLib;

    public override void OnStart()
    {
        PerlinXOffset = Random.Range(-10000, 10000);
        PerlinYOffset = Random.Range(-10000, 10000);
        rooms = new Room[RoomAmount];
        watenkLib = new WatenkLib();
        base.OnStart();

        GenerateTerrain();
        GenerateRooms();
        GenerateCorridors();
        gridRenderer.Draw();
    }

    private void GenerateTerrain()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                gridArray[x, y] = new Tile((ID)GetPerlinIntValue(x, y, WorldGenerationTiles));
            }
        }
    }

    private void GenerateRooms()
    {
        int failedRooms = 0;
        for (int i = 0; i < RoomAmount; i++)
        {
            int currentRetryFailedRoomGeneration = 0;

            startRoomGeneration:
            int xStartLocation = watenkLib.GetRandomInt(0, Width);
            int yStartLocation = watenkLib.GetRandomInt(0, Height);
            int xRoomSize = watenkLib.GetRandomInt(MinRoomSize, MaxRoomSize);
            int yRoomSize = watenkLib.GetRandomInt(MinRoomSize, MaxRoomSize);

            if (AreTilesAvailible(xStartLocation - RoomAvoidEdges, yStartLocation - RoomAvoidEdges, xStartLocation + xRoomSize + RoomAvoidEdges, yStartLocation + yRoomSize + RoomAvoidEdges, RoomTiles))
            {
                SetTiles(xStartLocation, yStartLocation, xStartLocation + xRoomSize, yStartLocation + yRoomSize, RoomsFloor, false);
                rooms[i] = new Room(xStartLocation, yStartLocation, xStartLocation + xRoomSize, yStartLocation + yRoomSize);
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
                    rooms[i] = null;
                    failedRooms++;
                }
            }
        }
        if (failedRooms != 0)
        {
            Debug.Log(failedRooms + " Room(s) Failed to Generate");
        }
    }

    private void GenerateCorridors()
    {
        //Need to implement
    }

    private int GetPerlinIntValue(float x, float y, ID[] tiles)
    {
        float normalizedPerlin = Mathf.PerlinNoise((x + PerlinXOffset) / PerlinMagnification, (y + PerlinYOffset) / PerlinMagnification); //Generate normalized value
        float tileIDPerlin = Mathf.Clamp(normalizedPerlin, 0.0f, 1.0f) * tiles.Length + 1; //Clamp all values between 0.0 and 1.0 and multiply with tileAmount
        tileIDPerlin = Mathf.Clamp(tileIDPerlin, 1.0f, tiles.Length); //Clamp between 0 and tileAmount
        return Mathf.FloorToInt(tileIDPerlin); //Return int
    }
}