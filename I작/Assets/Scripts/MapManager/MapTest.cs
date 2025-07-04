using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTest : MonoBehaviour
{
    public GameObject miniMapPrefab;
    public GameObject roomPrefab;        // 생성할 방 프리팹
    public Transform roomParent;         // 방들을 담을 부모 오브젝트 (계층 정리용)
    public int roomCount = 10;           // 생성할 방 개수
    public int mapSize = 8;              // 8x8 맵

    private int[,] s_roomGraph;
    private List<RoomClass> Rooms = new List<RoomClass>();
    private Queue<RoomClass> roomClassQueue = new Queue<RoomClass>();

    private int roomCnt = 0;
    private int s_mapMaxX, s_mapMaxY;
    private float tileWidth, tileHeight;
    private float spriteWidth, spriteHeight;

    void Start()
    {
        InitTileSize();
        InitMap();
        GenerateRooms();
        MarkEndRoomsAndBoss();
    }

    void InitTileSize()
    {
        Tilemap tilemap = roomPrefab.GetComponentInChildren<Tilemap>();
        if (tilemap != null)
        {
            Bounds bounds = tilemap.localBounds;
            tileWidth = bounds.size.x;
            tileHeight = bounds.size.y;
        }

        SpriteRenderer roomSprite = miniMapPrefab.GetComponentInChildren<SpriteRenderer>();
        if (roomSprite != null)
        {
            Bounds bounds = roomSprite.localBounds;
            Vector3 scale = roomSprite.transform.lossyScale;
            spriteWidth = bounds.size.x * scale.x;
            spriteHeight = bounds.size.y * scale.y;
        }
        
    }


    void InitMap()
    {
        s_mapMaxX = mapSize;
        s_mapMaxY = mapSize;
        s_roomGraph = new int[s_mapMaxX, s_mapMaxY];

        int centerX = s_mapMaxX / 2;
        int centerY = s_mapMaxY / 2;

        RoomClass startRoom = new RoomClass(centerX,centerY);

        Rooms.Add(startRoom);
        s_roomGraph[centerX, centerY] = 1;
        roomCnt++;

        CreateRoomObject(centerX, centerY);
    }

    void GenerateRooms()
    {
        while (roomCnt < roomCount)
        {
            roomClassQueue.Enqueue(Choice(Rooms));

            while (roomClassQueue.Count > 0)
            {
                RoomClass front = roomClassQueue.Dequeue();
                int[] dx = { 0, 1, 0, -1 };
                int[] dy = { 1, 0, -1, 0 };

                for (int i = 0; i < 4; i++)
                {
                    if (roomCnt >= roomCount) break;

                    int nx = front.XPos + dx[i];
                    int ny = front.YPos + dy[i];

                    if (CanCreateRoom(nx, ny))
                    {
                        RoomClass newRoom = new RoomClass(nx,ny);

                        front._adjacencentRooms[i] = newRoom;
                        newRoom._adjacencentRooms[(i + 2) % 4] = front;

                        s_roomGraph[nx, ny] = 1;

                        Rooms.Add(newRoom);
                        roomCnt++;
                        roomClassQueue.Enqueue(newRoom);

                        CreateRoomObject(nx, ny);
                    }
                }
            }
        }
    }

    void CreateRoomObject(int x, int y)
    {  
        // 중심 좌표를 기준으로 오프셋 계산
        int centerX = mapSize / 2;
        int centerY = mapSize / 2;

        float offsetX = (x - centerX) * tileWidth;
        float offsetY = (y - centerY) * tileHeight;

        Vector3 position = new Vector3(offsetX, offsetY, 0);
        GameObject go = Instantiate(roomPrefab, position, Quaternion.identity, roomParent);
        go.name = $"Room ({x},{y})";

        Vector3 spritePosition = new Vector3(x * spriteWidth + 100, y * spriteHeight + 100, 0);
        GameObject spriteGo = Instantiate(miniMapPrefab, spritePosition, Quaternion.identity, roomParent);
    }

    bool CanCreateRoom(int x, int y)
    {
        if (x < 0 || x >= s_mapMaxX || y < 0 || y >= s_mapMaxY)
            return false;

        if (s_roomGraph[x, y] == 1)
            return false;

        if (CheckAdjacentRoomCount(x, y) >= 2)
            return false;

        if (Random.Range(0, 100) >= 50)
            return false;

        return true;
    }

    int CheckAdjacentRoomCount(int x, int y)
    {
        int count = 0;
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx >= 0 && nx < s_mapMaxX && ny >= 0 && ny < s_mapMaxY)
            {
                if (s_roomGraph[nx, ny] == 1)
                    count++;
            }
        }

        return count;
    }

    RoomClass Choice(List<RoomClass> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    //  끝방 모두 찾아 표시하고, 가장 먼 끝방은 보스룸으로
    void MarkEndRoomsAndBoss()
    {
        RoomClass startRoom = Rooms[0];
        var result = GetEndRoomsAndFarthest(startRoom);

        foreach (var endRoom in result.allEndRooms)
        {
            //HighlightRoom(endRoom, Color.blue); // 파란색: 일반 끝방
        }

        if (result.farthestEndRoom != null)
        {
            result.farthestEndRoom.Type = RoomType.Boss;
            //HighlightRoom(result.farthestEndRoom, Color.red); // 빨간색: 가장 먼 끝방 = 보스
        }
    }

    //  끝방 모두 + 가장 먼 끝방 찾기
    TestEndRoomResult GetEndRoomsAndFarthest(RoomClass startRoom)
    {
        Queue<(RoomClass room, int depth)> queue = new Queue<(RoomClass, int)>();
        HashSet<RoomClass> visited = new HashSet<RoomClass>();
        List<RoomClass> endRooms = new List<RoomClass>();
        RoomClass farthestEndRoom = null;
        int maxDepth = -1;

        queue.Enqueue((startRoom, 0));
        visited.Add(startRoom);

        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();

            int connectedCount = 0;

            for (int i = 0; i < 4; i++)
            {
                var next = current._adjacencentRooms[i];
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

        return new TestEndRoomResult
        {
            allEndRooms = endRooms,
            farthestEndRoom = farthestEndRoom
        };
    }

    // 색상 변경 유틸
    void HighlightRoom(RoomClass room, Color color)
    {
        foreach (Transform child in roomParent)
        {
            if (child.name == $"Room ({room.XPos},{room.YPos})")
            {
                Transform visual = child.GetChild(0);
                SpriteRenderer sr = visual.GetComponent<SpriteRenderer>();
                sr.color = color;
            }
        }
    }
}

//  끝방 정보 구조체
public class TestEndRoomResult
{
    public List<RoomClass> allEndRooms;
    public RoomClass farthestEndRoom;
}
