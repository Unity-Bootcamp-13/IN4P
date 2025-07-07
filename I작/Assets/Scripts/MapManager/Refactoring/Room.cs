using UnityEngine;

public enum RoomType
{
    Normal,
    Start,
    Boss,
    Treasure,
    Secret
}

public class Room
{
    public int XPos;
    public int YPos;
    public RoomType Type;
    public Room[] nextRooms = new Room[4];

    public Room(int x, int y, RoomType type)
    {
        XPos = x;
        YPos = y;
        Type = type;
    }
}