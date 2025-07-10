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

            DoorType doorType = GetDoorType(neighbor);            
            GameObject doorGo = Instantiate(doorPrefabs[0], doorSpawnPoints[i].position, doorSpawnPoints[i].rotation, doorSpawnPoints[i]);
            if (doorType == DoorType.Secret)
            {
                doorGo.SetActive(false);
            }
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = DoorType.Treasure;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
        int rand = Random.Range(0, 10);

        if(rand <= 9)
        {
            itemGenerator.GetRandomPassiveItem(transform.position);
        }
        else
        {
            itemGenerator.GetRandomActiveItem(transform.position);
        }

    }
}