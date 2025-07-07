using UnityEngine;

public class TreasureRoomController : RoomController
{
    protected override void Start()
    {
        base.Start();
        minimapSpriteRenderer.color = Color.yellow;

        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] == null)
                continue;

            doorList[i].OpenDoor();
            doorList[i].transform.GetChild(0).gameObject.SetActive(false);
            doorList[i].portalCollider.isTrigger = true;
        }

        isCleared = true;
    }

    public override void CreateDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            RoomController neighbor = nextRoomControllers[i];

            if (neighbor == null) continue;

            GameObject doorGo = Instantiate(doorPrefabs[0], doorSpawnPoints[i].position, doorSpawnPoints[i].rotation, doorSpawnPoints[i]);
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = DoorType.Treasure;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
        Debug.Log("아이템 생성");
    }
}