using UnityEngine;

public class StartRoomController : RoomController
{
    protected override void Start()
    {
        base.Start();
        SetMinimap();
        isCleared = true;
    }

    public override void CreateDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            RoomController neighbor = nextRoomControllers[i];

            if (neighbor == null) continue;

            DoorType doorType = GetDoorType(neighbor);
            GameObject doorPrefab = GetDoorPrefabByType(doorType);

            GameObject doorGo = Instantiate(doorPrefab, doorSpawnPoints[i].position, doorSpawnPoints[i].rotation, doorSpawnPoints[i]);
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = doorType;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
        Debug.Log("시작방은 컨텐츠 없음");
    }
}