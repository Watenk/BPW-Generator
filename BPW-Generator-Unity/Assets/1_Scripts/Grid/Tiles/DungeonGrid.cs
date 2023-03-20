using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGrid : TileGrid
{
    //Terrain
    public ID[] WorldGenerationTiles = new ID[1]; //What tiles are used for generation
    public float PerlinMagnification; //The higher the bigger the tileCollections
    //Rooms
    public List <ID> RoomTiles = new List<ID>(); //Tiles where rooms can generate on
    public List<ID> walkableTiles = new List<ID>(); //Tiles where entities can move to/on
    public ID RoomsFloor;
    public int RoomAmount;
    public int MinRoomSize;
    public int MaxRoomSize;
    public int MaxRoomConnectDistance; //Max distance a room will search for another room to connect to
    public int RoomAvoidEdges; //Amount of tiles room will not generate from edge RoomTiles
    public int RetryFailedRoomGeneration; //Amount of times a room will trt to regenerate
    public int PlayerSpawnRadius; //Radius around 0, 0 the player can spawn

    private List<Alive> entitys = new List<Alive>();
    private List<Room> rooms = new List<Room>(); //List of rooms in map
    private float perlinXOffset;
    private float perlinYOffset;

    //References
    public GameObject PlayerPrefab;
    private GameManager gameManager;
    private Inputs inputs;
    private WatenkLib watenkLib;
    private AStar aStar;

    public override void OnAwake()
    {
        gameManager = FindObjectOfType<GameManager>();
        inputs = FindObjectOfType<Inputs>();
    }

    public override void OnStart()
    {
        watenkLib = new WatenkLib();
        aStar = new AStar();
        base.OnStart();

        CalcPerlinOffsets();
        GenerateTerrain();
        GenerateRooms();
        GenerateCorridors();
        SpawnObjects();
        SpawnEntities();
        gridRenderer.Draw();
    }

    public Alive GetEntity(int ID)
    {
        for (int i = 0; i < entitys.Count; i++)
        {
            if (entitys[i].GetID() == ID)
            {
                return entitys[i];
            }
        }
        return null;
    }

    private void AddEntity(GameObject prefab, Vector2Int pos, int ID)
    {
        Alive currentEntity = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
        gameManager.AddObject(currentEntity);
        entitys.Add(currentEntity);
        currentEntity.SetID(ID);
        currentEntity.SetPos(pos);
    }

    private Room GetClosestRoom(Room room)
    {
        Vector2Int currentRoomPos = room.GetMiddle();
        Room closestRoom = null;
        int closestDistance = MaxRoomConnectDistance;

        for (int i = 0; i < rooms.Count; i++)
        {
            int currentDistance = (int)Vector2Int.Distance(currentRoomPos, rooms[i].GetMiddle());
            if (currentDistance < closestDistance && rooms[i] != room)
            {
                closestDistance = currentDistance;
                closestRoom = rooms[i];
            }
        }
        return closestRoom;
    }

    private void CalcPerlinOffsets()
    {
        perlinXOffset = Random.Range(-10000, 10000);
        perlinYOffset = Random.Range(-10000, 10000);
    }

    private void SpawnObjects()
    {
        //Need to implement
    }

    private void SpawnEntities()
    {
        Vector2Int spawnPos = FindRandomFreeSpace(0, 0, PlayerSpawnRadius, PlayerSpawnRadius, walkableTiles).GetPos();
        AddEntity(PlayerPrefab, spawnPos, 1);
        inputs.FocusOnPlayer();
    }

    private void GenerateTerrain()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                gridArray[x, y] = new Tile((ID)GetPerlinIntValue(x, y, WorldGenerationTiles), new Vector2Int(x, y));
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
                rooms.Add(new Room(xStartLocation, yStartLocation, xStartLocation + xRoomSize, yStartLocation + yRoomSize));
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
                    failedRooms++;
                }
            }
        }
        Debug.Log("Generated " + rooms.Count + " Room(s), " + failedRooms + " Rooms failed");
    }

    private void GenerateCorridors()
    {
        int failedCorridors = 0;
        int corridorsGenerated = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            Room currentRoom = rooms[i];
            Room closestRoom = GetClosestRoom(currentRoom);

            if (closestRoom != null && closestRoom != currentRoom)
            {
                Vector2Int currentRoomPos = currentRoom.GetRandomPos();
                Vector2Int closestRoomPos = closestRoom.GetRandomPos();
                Tile currentRoomTile = GetTile(currentRoomPos.x, currentRoomPos.y);
                Tile closestRoomTile = GetTile(closestRoomPos.x, closestRoomPos.y);
                List<Tile> fastestPath = aStar.CalcPath(currentRoomTile, closestRoomTile, this);

                if (fastestPath != null)
                {
                    for (int j = 0; j < fastestPath.Count; j++)
                    {
                        Vector2Int currentTilePos = fastestPath[j].GetPos();
                        SetTile(currentTilePos.x, currentTilePos.y, ID.pavedStone, false);
                    }
                    corridorsGenerated++;
                }
                else
                {
                    failedCorridors++;
                }
            }
            else
            {
                failedCorridors++;
            }
        }
        Debug.Log("Generated " + corridorsGenerated + " Corridor(s), " + failedCorridors + " Corridors failed");
    }

    private int GetPerlinIntValue(float x, float y, ID[] tiles)
    {
        float normalizedPerlin = Mathf.PerlinNoise((x + perlinXOffset) / PerlinMagnification, (y + perlinYOffset) / PerlinMagnification); //Generate normalized value
        float tileIDPerlin = Mathf.Clamp(normalizedPerlin, 0.0f, 1.0f) * tiles.Length + 1; //Clamp all values between 0.0 and 1.0 and multiply with tileAmount
        tileIDPerlin = Mathf.Clamp(tileIDPerlin, 1.0f, tiles.Length); //Clamp between 0 and tileAmount
        return Mathf.FloorToInt(tileIDPerlin); //Return int
    }
}