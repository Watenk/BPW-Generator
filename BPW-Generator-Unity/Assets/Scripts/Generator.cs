using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SimpleDungeon
{
    public enum TileType { Floor, Wall }

    public class Generator : MonoBehaviour
    {
        public GameObject floorPrefab;
        public GameObject wallPrefab;
        public int gridWidth = 100;
        public int gridHeight = 100;
        public int minRoomSize = 3;
        public int maxRoomSize = 7;
        public int numRooms = 10;
        public Dictionary<Vector3Int, TileType> dungeon = new Dictionary<Vector3Int, TileType>();
        public List<Room> roomList = new List<Room>();
        public List<GameObject> allInstantiatedPrefabs = new List<GameObject>();

        void Start()
        {
            Generate();
        }
        /// <summary>
        /// Generates the dungeon
        /// </summary>
        [ContextMenu("Generate Dungeon")]

        public void Generate()
        {
            Debug.Log("Start Generating");
            //Rooms allocaten
            //Connect Rooms with corridors
            //Generate the dungeon
            //Doors?
            //Remove double corridors
            //Add Enemies
            //Add loot
            //Add Player
            ClearDungeon();
            AllocateRooms();
            ConnectRooms();
            AllocateWalls();
            SpawnDungeon();
        }

        [ContextMenu("Clear Dungeon")]

        public void ClearDungeon()
        {
            for (int i = allInstantiatedPrefabs.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(allInstantiatedPrefabs[i]);
            }
            dungeon.Clear();
            roomList.Clear();
        }

        private void ConnectRooms()
        {
            // [0, 1, 2, {3}, 4, 5] 0, 1, 2
            for (int i = 0; i < roomList.Count; i++)
            {
                Room room = roomList[i];
                Room otherRoom = roomList[(i + Random.Range(1, roomList.Count)) % roomList.Count];
                ConnectRooms(room, otherRoom);
            }
        }

        private void AllocateRooms()
        {
            for (int i = 0; i < numRooms; i++)
            {
                int minX = Random.Range(0, gridWidth);
                int maxX = minX + Random.Range(minRoomSize, maxRoomSize + 1);
                int minZ = Random.Range(0, gridHeight);
                int maxZ = minZ + Random.Range(minRoomSize, maxRoomSize + 1);
                Room room = new Room(minX, maxX, minZ, maxZ);
                if (CanRoomFitInDungeon(room))
                {
                    AddRoomToDungeon(room);
                }
                else
                {
                    i--;
                }
            }
        }

        public void AllocateWalls()
        {
            var keys = dungeon.Keys.ToList();
            foreach (var kv in keys)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        //if(Mathf.Abs(x) == Mathf.Abs(z)) { continue; }
                        Vector3Int newPos = kv + new Vector3Int(x, 0, z);
                        if (dungeon.ContainsKey(newPos)) { continue; }
                        dungeon.Add(newPos, TileType.Wall);
                    }
                }
            }
        }

        public void ConnectRooms(Room _roomOne, Room _roomTwo)
        {
            Vector3Int posOne = _roomOne.GetCenter();
            Vector3Int posTwo = _roomTwo.GetCenter();
            int dirX = posTwo.x > posOne.x ? 1 : -1;
            int x = 0;
            for (x = posOne.x; x != posTwo.x; x += dirX)
            {
                Vector3Int position = new Vector3Int(x, 0, posOne.z);
                if (dungeon.ContainsKey(position)) { continue; }
                dungeon.Add(position, TileType.Floor);
            }
            int dirZ = posTwo.z > posOne.z ? 1 : -1;
            for (int z = posOne.z; z != posTwo.z; z += dirZ)
            {
                Vector3Int position = new Vector3Int(x, 0, z);
                if (dungeon.ContainsKey(position)) { continue; }
                dungeon.Add(position, TileType.Floor);
            }
        }

        public void SpawnDungeon()
        {
            foreach (KeyValuePair<Vector3Int, TileType> kv in dungeon)
            {
                GameObject obj = null;
                switch (kv.Value)
                {
                    case TileType.Floor:
                        obj = Instantiate(floorPrefab, kv.Key, Quaternion.identity, transform); break;
                    case TileType.Wall:
                        obj = Instantiate(wallPrefab, kv.Key, Quaternion.identity, transform); break;
                }
                allInstantiatedPrefabs.Add(obj);
            }
        }

        public void AddRoomToDungeon(Room room)
        {
            for (int x = room.minX; x <= room.maxX; x++)
            {
                for (int z = room.minZ; z <= room.maxZ; z++)
                {
                    dungeon.Add(new Vector3Int(x, 0, z), TileType.Floor);
                }
            }
            roomList.Add(room);
        }

        public bool CanRoomFitInDungeon(Room room)
        {
            for (int x = room.minX - 1; x <= room.maxX + 1; x++)
            {
                for (int z = room.minZ - 1; z <= room.maxZ + 1; z++)
                {
                    if (dungeon.ContainsKey(new Vector3Int(x, 0, z)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public class Room
    {
        public int minX, maxX, minZ, maxZ;

        public Room(int _minX, int _maxX, int _minZ, int _maxZ)
        {
            minX = _minX;
            maxX = _maxX;
            minZ = _minZ;
            maxZ = _maxZ;
        }

        public Vector3Int GetCenter()
        {
            return new Vector3Int(Mathf.RoundToInt(Mathf.Lerp(minX, maxX, 0.5f)), 0, Mathf.RoundToInt(Mathf.Lerp(minZ, maxZ, 0.5f)));
        }

        public Vector3Int GetRandomPositionInRoom()
        {
            return new Vector3Int(Random.Range(minX, maxX + 1), 0, Random.Range(minZ, maxZ + 1));
        }
    }
}