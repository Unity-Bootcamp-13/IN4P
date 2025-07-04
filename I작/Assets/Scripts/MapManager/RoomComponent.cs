using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RoomComponent : MonoBehaviour
{
    public Transform[] doorSpawnPoints = new Transform[4]; // »ó¿ìÇÏÁÂ
    public GameObject[] doorPrefabs;
    public bool playerIsHere;
    

    private RoomClass roomData;
    private GameObject[] doorList = new GameObject[4];

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            OpenDoors();
        }
    }

    public void Init(RoomClass data)
    {
        roomData = data;
    }

    public void CreateDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            var neighbor = roomData._adjacencentRooms[i];
            if (neighbor == null) continue;

            DoorType doorType = GetDoorTypeTo(neighbor);
            GameObject prefab = GetDoorPrefabByType(doorType);

            if (prefab == null || doorSpawnPoints[i] == null) continue;

            GameObject doorGo = Instantiate(prefab, doorSpawnPoints[i].position, doorSpawnPoints[i].rotation, doorSpawnPoints[i]);
            doorList[i] = doorGo;
            doorGo.GetComponent<Portal>().thisDirction = i;
            doorGo.GetComponent<Portal>().portalAction += SetCharacter;
        }
    }

    private void SetCharacter(int direction)
    {
        roomData._adjacencentRooms[direction].childObject.GetComponentInParent<RoomComponent>().CloseDoors();
    }

    private DoorType GetDoorTypeTo(RoomClass neighbor)
    {
        return neighbor.Type switch
        {
            RoomType.Boss => DoorType.Boss,
            RoomType.Treasure => DoorType.Treasure,
            _ => DoorType.Normal,
        };
    }

    private GameObject GetDoorPrefabByType(DoorType type)
    {
        // ¿¹½Ã: index 0 = Normal, 1 = Boss, 2 = Treasure
        return doorPrefabs[(int)type];
    }

    public void CloseDoors()
    {
        Debug.Log("¹®´ÝÈû");
        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] != null)
            {
                doorList[i].GetComponent<Portal>().CloseDoor();
            }
        }
    }

    public void OpenDoors()
    {
        Debug.Log("¹®¿­¸²");
        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] != null)
            {
                doorList[i].GetComponent<Portal>().OpenDoor();
            }
        }
    }
}
