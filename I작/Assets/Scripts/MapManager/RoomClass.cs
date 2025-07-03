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

    // Ȯ��: Room Ÿ�� (��: �Ϲ�, ����, ���� ��)
    public RoomType Type = RoomType.Normal;

    public RoomClass(int x, int y)
    {
        XPos = x;
        YPos = y;
    }
}