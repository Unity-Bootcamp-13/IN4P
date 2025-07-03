using UnityEngine;

public enum RoomType
{
    Normal,
    Start,
    End,
    Boss,
    Treasure,
    Event
}

public class RoomClass
{
    public int XPos;
    public int YPos;

    // 0: Right, 1: Down, 2: Left, 3: Up
    public RoomClass[] _adjacencentRooms = new RoomClass[4];

    public GameObject VisualObject;

    // 확장: Room 타입 (예: 일반, 보스, 상점 등)
    public RoomType Type = RoomType.Normal;

    public RoomClass(int x, int y)
    {
        XPos = x;
        YPos = y;
    }
}