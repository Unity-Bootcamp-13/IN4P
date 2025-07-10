using UnityEngine;

public class SecretRoomController : RoomController
{
    [SerializeField] GameObject BrimStone;

    protected override void Start()
    {
        base.Start();
        minimapSpriteRenderer.color = Color.black;
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
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = DoorType.Secret;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
        Instantiate(BrimStone, transform.position, Quaternion.identity);
    }
}