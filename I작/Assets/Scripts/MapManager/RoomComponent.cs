using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RoomComponent : MonoBehaviour
{
    public Camera mainCamera;
    public Transform[] doorSpawnPoints = new Transform[4]; // 상우하좌
    public GameObject[] doorPrefabs;
    public bool playerIsHere;
    public int monsterCount = 5;

    private RoomClass roomData;
    private GameObject[] doorList = new GameObject[4];

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(monsterCount);
            monsterCount--;

            if (monsterCount == 0)
            {
                OpenDoors();
            }
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
            doorGo.GetComponent<Portal>().portalAction += SetRoomState;
        }
    }

    private void SetRoomState(int direction)
    {
        RoomComponent nextRoom = roomData._adjacencentRooms[direction].childObject.GetComponentInParent<RoomComponent>();
        nextRoom.CloseDoors();

        StartCoroutine(C_CameraMove(nextRoom));
    }

    public void SetMiniMap(RoomComponent nextRoom)
    {
        //nextRoom.roomData.miniMapSprite.color.

        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] != null)
            {
                doorList[i].GetComponent<Portal>().OpenDoor();
            }
        }
    }

    public IEnumerator C_CameraMove(RoomComponent nextRoom)
    {
        float t = 0.5f;
        float elapse = 0f;
        Vector3 startPos = mainCamera.transform.position;
        Vector3 endPos = nextRoom.transform.position + new Vector3(0,0,-10);
        while (elapse <= t)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, elapse/t);
            elapse += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = endPos;
        
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
        // 예시: index 0 = Normal, 1 = Boss, 2 = Treasure
        return doorPrefabs[(int)type];
    }

    public void CloseDoors()
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] != null)
            {
                doorList[i].GetComponent<Portal>().CloseDoor();
                monsterCount = 5;
            }
        }
    }

    public void OpenDoors()
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] != null)
            {
                doorList[i].GetComponent<Portal>().OpenDoor();
            }
        }
    }

}
