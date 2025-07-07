using UnityEngine;

public enum LegacyRoomType
{
    Normal,
    Start,
    End,
    Boss,
    Treasure,
    Event
}

public enum LegaacyDoorType
{
    Normal,
    Boss,
    Treasure,
}

public class RoomClass
{
    public int XPos;
    public int YPos;

    // 0: Right, 1: Down, 2: Left, 3: Up
    public RoomClass[] _adjacencentRooms = new RoomClass[4];

    public GameObject childObject;
    public SpriteRenderer miniMapSprite;

    // 확장: Room 타입 (예: 일반, 보스, 상점 등)
    public LegacyRoomType Type;

    public RoomClass(int x, int y)
    {
        XPos = x;
        YPos = y;
    }
}