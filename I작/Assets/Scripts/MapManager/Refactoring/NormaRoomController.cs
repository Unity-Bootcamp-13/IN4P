using UnityEngine;

public class NormalRoomController : RoomController
{
    public GameObject[] monsterContents;
    public Transform[] spawnPoints;
    private int monsterCount;

    protected override void Start()
    {
        base.Start();
        monsterCount = monsterContents.Length;
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
            if (doorType == DoorType.Secret)
            {
                doorGo.SetActive(false);  // ���߿� ������ ���� ���� ����
            }
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = doorType;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }
    protected override void GenerateContents()
    {
        for (int i = 0; i < monsterContents.Length;i++)
        {
            if (monsterContents == null)
            {
                isCleared = true;
                return;
            }

            GameObject monsterGo = Instantiate(monsterContents[i], spawnPoints[i].position, Quaternion.identity);
            // Monster Ŭ������ Action ���� �ϳ� ���� CheckClearCondition ����ϰ� Monster ���� �� ���� invoke
        }
    }

    public void CheckClearCondition()
    {
        if (--monsterCount == 0)
            isCleared = true;
            OpenDoors();
    }
}