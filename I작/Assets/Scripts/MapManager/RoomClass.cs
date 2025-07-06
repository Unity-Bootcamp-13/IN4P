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

    // Ȯ��: Room Ÿ�� (��: �Ϲ�, ����, ���� ��)
    public LegacyRoomType Type;

    public RoomClass(int x, int y)
    {
        XPos = x;
        YPos = y;
    }
}