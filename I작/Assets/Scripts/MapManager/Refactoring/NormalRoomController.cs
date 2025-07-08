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
                doorGo.SetActive(false);  // 나중에 열리게 만들 수도 있음
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
            // Monster 클래스에 Action 변수 하나 만들어서 CheckClearCondition 등록하고 Monster 죽을 때 마다 invoke
        }
    }

    public void CheckClearCondition()
    {
        if (--monsterCount == 0)
            isCleared = true;
            OpenDoors();
    }
}