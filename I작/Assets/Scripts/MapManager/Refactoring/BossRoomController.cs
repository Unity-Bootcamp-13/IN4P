using UnityEngine;

public class BossRoomController : RoomController
{
    public GameObject bossContent;
    public Transform spawnPoint;

    protected override void Start()
    {
        base.Start();
        minimapSpriteRenderer.color = Color.red;
    }

    public override void CreateDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            RoomController neighbor = nextRoomControllers[i];

            if (neighbor == null) continue;

            GameObject doorGo = Instantiate(doorPrefabs[0], doorSpawnPoints[i].position, doorSpawnPoints[i].rotation, doorSpawnPoints[i]);
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = DoorType.Boss;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
        GameObject bossGo = Instantiate(bossContent, spawnPoint.position, Quaternion.identity);
        //bossGo.GetComponent<Monstro>().deadAction += CheckClearCondition;
    }

    public void CheckClearCondition()
    {
        isCleared = true;
        OpenDoors();
    }
}