using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGrid : TileGrid
{
    [Header("Terrain")]
    public ID[] WorldGenerationTiles = new ID[1]; //What tiles are used for generation
    public float PerlinMagnification; //The higher the bigger the tileCollections

    [Header("Rooms")]
    public List <ID> RoomTiles = new List<ID>(); //Tiles where rooms can generate on
    public List<ID> walkableTiles = new List<ID>(); //Tiles where entities can move to/on
    public List<ID> corridorGeneratableTiles = new List<ID>(); //Tiles where corridors can generate on
    public ID RoomsFloor;
    public int RoomAmount;
    public int MinRoomSize;
    public int MaxRoomSize;
    public int MaxRoomConnectDistance; //Max distance a room will search for another room to connect to
    public int RoomAvoidEdges; //Amount of tiles room will not generate from edge RoomTiles
    public int RetryFailedRoomGeneration; //Amount of times a room will trt to regenerate

    [Header("Entities")]
    public int PlayerSpawnRadius; //Radius around 0, 0 the player can spawn
    public int EnemySpawnChance; //Percentage a enemy will spawn
    public int EnemyMaxSpawnAmount; //Max amount of enemys per room

    [Header("Prefabs")]
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    private List<Alive> entitys = new List<Alive>();
    private List<Room> rooms = new List<Room>(); //List of rooms in map
    private float perlinXOffset;
    private float perlinYOffset;
    private int nextEntityID;

    //References
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
        SpawnEntities();
        SpawnObjects();
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

    public Alive GetEntity(Vector2Int pos)
    {
        for (int i = 0; i < entitys.Count; i++)
        {
            if (entitys[i].GetPos() == pos)
            {
                return entitys[i];
            }
        }
        return null;
    }

    private void SpawnObjects()
    {
        //Need to implement
    }

    private void SpawnEntities()
    {
        //Player
        List<ID> playerSpawnTiles = new List<ID>() { ID.grass };
        Vector2Int playerSpawnPos = FindRandomFreeSpace(0, 0, PlayerSpawnRadius, PlayerSpawnRadius, playerSpawnTiles).GetPos();
        AddEntity(PlayerPrefab, playerSpawnPos);
        inputs.FocusOnPlayer();

        //Enemy's
        for (int i = 0; i < rooms.Count; i++)
        {
            int spawnPercentage = Random.Range(1, 100);
            if (spawnPercentage <= EnemySpawnChance)
            {
                Room currentRoom = rooms[i];
                
                int enemyAmount = Random.Range(1, EnemyMaxSpawnAmount);
                
                for (int j = 0; j < enemyAmount; j++)
                {
                    retry:
                    Vector2Int currentEnemySpawnPos = currentRoom.GetRandomPos();
                    if (GetEntity(currentEnemySpawnPos) == null)
                    {
                        AddEntity(EnemyPrefab, currentEnemySpawnPos);   
                    }
                    else
                    {
                        goto retry;
                    }
                }
            }
        }
    }

    //Getters -----------------------------------------------------------

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

    public Room GetRandomRoom()
    {
        return rooms[Random.Range(0, rooms.Count)];
    }

    public Tile GetRandomTile(int x1, int y1, int x2, int y2, List<ID> allowedTiles)
    {
        int amountOfRetrys = Mathf.Abs(x2 - x1) * Mathf.Abs(y2 - y1);

        retry:
        int randomX = Random.Range(x1, x2);
        int randomY = Random.Range(y1, y2);

        Tile currentTile = GetTile(randomX, randomY);
        if (currentTile != null && allowedTiles.Contains(currentTile.GetID()))
        {
            return currentTile;
        }

        amountOfRetrys--;
        if (amountOfRetrys < 1)
        {
            return null;
        }
        goto retry;
    }

    public Tile GetRandomTile(List<ID> allowedTiles)
    {
        int amountOfRetrys = Width * Height;

        retry:
        int randomX = Random.Range(0, Width);
        int randomY = Random.Range(0, Height);

        Tile currentTile = GetTile(randomX, randomY);
        if (currentTile != null && allowedTiles.Contains(currentTile.GetID()))
        {
            return currentTile;
        }

        amountOfRetrys--;
        if (amountOfRetrys < 1)
        {
            return null;
        }
        goto retry;
    }

    private int GetPerlinIntValue(float x, float y, ID[] tiles)
    {
        float normalizedPerlin = Mathf.PerlinNoise((x + perlinXOffset) / PerlinMagnification, (y + perlinYOffset) / PerlinMagnification); //Generate normalized value
        float tileIDPerlin = Mathf.Clamp(normalizedPerlin, 0.0f, 1.0f) * tiles.Length + 1; //Clamp all values between 0.0 and 1.0 and multiply with tileAmount
        tileIDPerlin = Mathf.Clamp(tileIDPerlin, 1.0f, tiles.Length); //Clamp between 0 and tileAmount
        return Mathf.FloorToInt(tileIDPerlin); //Return int
    }

    //Setters ----------------------------------------------------------------------

    private void AddEntity(GameObject prefab, Vector2Int pos)
    {
        Alive currentEntity = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Alive>();
        gameManager.AddObject(currentEntity);
        entitys.Add(currentEntity);
        currentEntity.SetID(nextEntityID);
        nextEntityID++;
        currentEntity.SetPos(pos);
    }

    //Terrain / Dungeon generation -------------------------------------------------

    private void CalcPerlinOffsets()
    {
        perlinXOffset = Random.Range(-10000, 10000);
        perlinYOffset = Random.Range(-10000, 10000);
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
                List<Tile> fastestPath = aStar.CalcPath(currentRoomTile, closestRoomTile, this, corridorGeneratableTiles);

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
}