using System.Collections.Generic;
using UnityEngine;

public class NormalRoomController : RoomController
{
    public GameObject[] monsterContents;
    public List<GameObject> currentMonster;
    public Transform[] spawnPoints;
    public int monsterCount;
    [SerializeField] GameObject[] chest;

    protected override void Start()
    {
        base.Start();
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
        int monsterToSpawn = Random.Range(1, 7);

        if (monsterToSpawn == 0)
        {
            isCleared = true;
            OpenDoors();
            return;
        }

        monsterCount = monsterToSpawn;

        for (int i = 0; i < monsterToSpawn; i++)
        {
            int randomIndex = Random.Range(0, monsterContents.Length);
            GameObject prefab = monsterContents[randomIndex];
            Transform spawnPoint = spawnPoints[i];

            GameObject monsterGo = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            currentMonster.Add(monsterGo);

            Enemy enemy = monsterGo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.roomcontroller = this;
                enemy.OnDeath += CheckClearCondition;
            }
        }
        
    }

    public void CheckClearCondition()
    {
        monsterCount--;

        if (monsterCount == 0)
        {
            isCleared = true;
            OpenDoors();
            int rand = Random.Range(0, 10);

            if(rand <= 3)
            {
                itemGenerator.GetRandomPickupItem(transform.position);
            }
            else if(rand <= 6)
            {
                Instantiate(chest[0], transform.position, Quaternion.identity);
            }
            else if(rand <= 7)
            {
                Instantiate(chest[1], transform.position, Quaternion.identity);
            }
        }
    }


}