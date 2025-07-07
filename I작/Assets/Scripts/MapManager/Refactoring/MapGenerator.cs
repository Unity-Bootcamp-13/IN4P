using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemapSize;
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;
    public GameObject normalRoomPrefab;
    public GameObject treasureRoomPrefab;
    public Transform roomParent;
    
    public int targetRoomCount = 10;
    public int mapSize = 8;

    private int[,] roomGraph;
    public List<Room> Rooms = new();
    private Queue<Room> roomQueue = new();
    public Dictionary<Room, RoomController> roomToController = new();

    private int genereatedRoomCount = 0;
    private int mapMaxX, mapMaxY;
    private float tileWidth, tileHeight;

    void Start()
    {
        InitTileSize();
        InitMap();
        GenerateRooms();
        MarkEndRoomsAndBoss();
        ConnectRoomControllers();
        CreateDoor();
    }

    void InitTileSize()
    {
        Bounds tileBounds = tilemapSize.localBounds;
        tileWidth = tileBounds.size.x;
        tileHeight = tileBounds.size.y;
    }


    void InitMap()
    {
        mapMaxX = mapSize;
        mapMaxY = mapSize;
        roomGraph = new int[mapMaxX, mapMaxY];

        int centerX = mapMaxX / 2;
        int centerY = mapMaxY / 2;

        Room startRoom = new Room(centerX, centerY, RoomType.Start);
        Rooms.Add(startRoom);
        roomGraph[centerX, centerY] = 1;
        genereatedRoomCount++;

        CreateRoomObject(centerX, centerY, startRoom);
    }

    void GenerateRooms()
    {
        while (genereatedRoomCount < targetRoomCount)
        {
            roomQueue.Enqueue(Choice(Rooms));

            while (roomQueue.Count > 0)
            {
                Room front = roomQueue.Dequeue();
                int[] dx = { 0, 1, 0, -1 };
                int[] dy = { 1, 0, -1, 0 };

                for (int i = 0; i < 4; i++)
                {
                    if (genereatedRoomCount >= targetRoomCount) break;

                    int nx = front.XPos + dx[i];
                    int ny = front.YPos + dy[i];

                    if (CanCreateRoom(nx, ny))
                    {
                        Room newRoom = new Room(nx, ny, RoomType.Normal);
                        front.nextRooms[i] = newRoom;
                        newRoom.nextRooms[(i + 2) % 4] = front;

                        roomGraph[nx, ny] = 1;

                        Rooms.Add(newRoom);
                        genereatedRoomCount++;
                        roomQueue.Enqueue(newRoom);

                        CreateRoomObject(nx, ny, newRoom);
                    }
                }
            }
        }
    }

    void CreateRoomObject(int x, int y, Room room)
    {
        int centerX = mapSize / 2;
        int centerY = mapSize / 2;

        float offsetX = (x - centerX) * tileWidth;
        float offsetY = (y - centerY) * tileHeight;

        Vector3 position = new Vector3(offsetX, offsetY, 0);
        GameObject go = Instantiate(GetRoomPrefabByType(room.Type), position, Quaternion.identity, roomParent);
        go.name = $"Room ({x},{y})";
        RoomController newRoomController = go.GetComponent<RoomController>();
        newRoomController.Init(room);
        roomToController.Add(room, newRoomController);
    }

    Room Choice(List<Room> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    GameObject GetRoomPrefabByType(RoomType type)
    {
        switch (type)
        {
            case RoomType.Start:
                return startRoomPrefab;
            case RoomType.Boss:
                return bossRoomPrefab;
            case RoomType.Treasure:
                return treasureRoomPrefab;
            default:
                return normalRoomPrefab;
        }
    }

    bool CanCreateRoom(int x, int y)
    {
        if (x < 0 || x >= mapMaxX || y < 0 || y >= mapMaxY)
            return false;

        if (roomGraph[x, y] == 1)
            return false;

        if (CheckNextRoomCount(x, y) >= 2)
            return false;

        if (Random.Range(0, 100) >= 50)
            return false;

        return true;
    }

    int CheckNextRoomCount(int x, int y)
    {
        int count = 0;

        // 상, 우, 하, 좌
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx >= 0 && nx < mapMaxX && ny >= 0 && ny < mapMaxY)
            {
                if (roomGraph[nx, ny] == 1)
                    count++;
            }
        }

        return count;
    }
    void MarkEndRoomsAndBoss()
    {
        Room startRoom = Rooms[0];
        EndRooms result = GetEndRoomsAndFarthest(startRoom);
        result.allEndRooms.Remove(startRoom);

        if (result.farthestEndRoom != null)
        {
            Room bossRoom = result.farthestEndRoom;
            bossRoom.Type = RoomType.Boss;
            ReplaceRoom(bossRoom);
            result.allEndRooms.Remove(bossRoom);
        }

        int rand = Random.Range(0, result.allEndRooms.Count);
        Room randomRoom = result.allEndRooms[rand];
        randomRoom.Type = RoomType.Treasure;
        ReplaceRoom(randomRoom);
    }

    void ReplaceRoom(Room room)
    {
        if (roomToController.TryGetValue(room, out RoomController controller))
        {
            Destroy(controller.gameObject);
            roomToController.Remove(room);
        }

        CreateRoomObject(room.XPos, room.YPos, room);
    }

    EndRooms GetEndRoomsAndFarthest(Room startRoom)
    {
        Queue<(Room room, int depth)> queue = new Queue<(Room, int)>();
        HashSet<Room> visited = new HashSet<Room>();
        List<Room> endRooms = new List<Room>();
        Room farthestEndRoom = null;
        int maxDepth = -1;

        queue.Enqueue((startRoom, 0));
        visited.Add(startRoom);

        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();

            int connectedCount = 0;

            for (int i = 0; i < 4; i++)
            {
                var next = current.nextRooms[i];
                if (next != null)
                {
                    connectedCount++;

                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue((next, depth + 1));
                    }
                }
            }

            if (connectedCount == 1)
            {
                endRooms.Add(current);

                if (depth > maxDepth)
                {
                    maxDepth = depth;
                    farthestEndRoom = current;
                }
            }
        }

        return new EndRooms
        {
            allEndRooms = endRooms,
            farthestEndRoom = farthestEndRoom
        };
    }    

    void CreateDoor()
    {
        foreach (var keyValue in roomToController)
        {
            RoomController controller = keyValue.Value;

            controller.CreateDoors();
        }
    }

    void ConnectRoomControllers()
    {
        foreach (var kvp in roomToController)
        {
            Room room = kvp.Key;
            RoomController controller = kvp.Value;

            for (int i = 0; i < 4; i++)
            {
                Room adjacentRoom = room.nextRooms[i];
                if (adjacentRoom == null) continue;

                if (roomToController.TryGetValue(adjacentRoom, out RoomController neighborController))
                {
                    controller.nextRoomControllers[i] = neighborController;
                }
            }
        }
    }
}

public class EndRooms
{
    public List<Room> allEndRooms;
    public Room farthestEndRoom;
}
