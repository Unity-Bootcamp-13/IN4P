using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NormalRoomController : RoomController
{
    public GameObject[] monsterContents;
    public List<GameObject> currentMonster;
    //public float scanRadius = 11f; // �� Ÿ�Ͽ� ������Ʈ�� �ִ��� �˻��� ������
    private float tileWidth, tileHeight;
    public Transform[] spawnPoints;
    public int monsterCount;

    protected override void Start()
    {
        base.Start();
        // monsterCount = monsterContents.Length;
        // currentMonster = new List<GameObject>(); 
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
        int monsterToSpawn = Random.Range(0, 4); // 0~3�������� ����

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

            // Enemy�� OnDeath Action�� ���ǵǾ� �ִٰ� ����
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
        Debug.Log($"Monster died. Remaining: {monsterCount}");

        if (monsterCount == 0)
        {
            Debug.Log("All monsters cleared. Opening doors.");
            isCleared = true;
            OpenDoors();
        }
    }


}