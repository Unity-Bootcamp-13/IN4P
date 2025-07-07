using UnityEngine;

public class SecretRoomController : RoomController
{
    protected override void Start()
    {
        minimapSpriteRenderer.color = Color.black;



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

            DoorType doorType = GetDoorType(neighbor);


            GameObject doorGo = Instantiate(doorPrefabs[0], doorSpawnPoints[i].position, doorSpawnPoints[i].rotation, doorSpawnPoints[i]);
            if (doorType != DoorType.Secret)
            {
                doorGo.SetActive(false);  // 나중에 열리게 만들 수도 있음
            }
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = DoorType.Secret;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
    }
}