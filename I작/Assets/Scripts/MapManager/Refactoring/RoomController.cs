using System.Collections;
using UnityEngine;

public abstract class RoomController : MonoBehaviour
{
    private Camera mainCamera;
    public Transform[] doorSpawnPoints = new Transform[4];
    public GameObject[] doorPrefabs;
    protected Door[] doorList = new Door[4];
    
    public GameObject minimapObject;
    protected SpriteRenderer minimapSpriteRenderer;

    private Room roomData;
    public RoomController[] nextRoomControllers = new RoomController[4];

    public bool secretRoomVisited;


    protected bool isCleared;

    private void Awake()
    {
        mainCamera = Camera.main;
        GameObject minimapGo = Instantiate(minimapObject, transform.position + Vector3.up * 200, Quaternion.identity, transform);
        minimapSpriteRenderer = minimapGo.GetComponent<SpriteRenderer>();
        minimapSpriteRenderer.enabled = false;
    }

    protected virtual void Start()
    {
        OpenDoors();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            OpenDoors();
        }
        
        if(Input.GetKeyDown(KeyCode.V))
        {
            OpenSecretDoors();

        }        
    }

    public void Init(Room data)
    {
        roomData = data;
    }

    public abstract void CreateDoors();

    protected abstract void GenerateContents();

    protected void SetNextRoom(int direction)
    {
        RoomController nextRoom = nextRoomControllers[direction];

        if (!nextRoom.isCleared)
        {
            nextRoom.CloseDoors();
            nextRoom.GenerateContents();
        }
        //if(nextRoom.roomData.Type == RoomType.Secret)
        //{
        //
        //}

        nextRoom.SetMinimap();
        StartCoroutine(C_CameraMove(nextRoom));
    }

    public void OpenDoors()
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] == null)
                continue;

            doorList[i].OpenDoor();
        }
    }

    public void OpenSecretDoors()
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            Door door = doorList[i];
            RoomController neighbor = nextRoomControllers[i];

            if (door != null &&
                door.type == DoorType.Secret &&
                neighbor != null &&
                neighbor.roomData.Type == RoomType.Secret)
            {
                // 현재 방의 문 활성화 및 열기
                door.gameObject.SetActive(true);
                door.OpenDoor();

                int oppositeDir = (i + 2) % 4;

                // neighbor(비밀방)의 반대 방향 문과 연결된 방 확인
                if (neighbor.nextRoomControllers[oppositeDir] == this)
                {
                    Door neighborDoor = neighbor.doorList[oppositeDir];

                    if (neighborDoor != null && neighborDoor.type == DoorType.Secret)
                    {

                        neighborDoor.gameObject.SetActive(true);
                        neighborDoor.OpenDoor();
                    }
                }

                secretRoomVisited = true;
            }
        }
    }


    public void CloseDoors()
    {
        for (int i = 0; i < doorList.Length; i++)
        {
            if (doorList[i] == null)
                continue;

            doorList[i].CloseDoor();
        }
    }


    protected DoorType GetDoorType(RoomController next)
    {
        return next.roomData.Type switch
        {
            RoomType.Boss => DoorType.Boss,
            RoomType.Treasure => DoorType.Treasure,
            RoomType.Secret => DoorType.Secret,
            _ => DoorType.Normal,
        };
    }

    protected GameObject GetDoorPrefabByType(DoorType type)
    {
        return doorPrefabs[(int)type];
    }

    private IEnumerator C_CameraMove(RoomController room)
    {
        float t = 0.5f;
        float elapse = 0f;
        Vector3 startPos = mainCamera.transform.position;
        Vector3 endPos = room.transform.position + new Vector3(0,0,-10);

        while (elapse <= t)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, elapse/t);
            elapse += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = endPos;
        
    }

    protected  void SetMinimap()
    {
        minimapSpriteRenderer.enabled = true;

        // 현재 방은 항상 보이게
        Color currentColor = minimapSpriteRenderer.color;
        currentColor.a = 1f;
        minimapSpriteRenderer.color = currentColor;

        for (int i = 0; i < nextRoomControllers.Length; i++)
        {
            RoomController neighbor = nextRoomControllers[i];
            if (neighbor == null) continue;

            neighbor.minimapSpriteRenderer.enabled = false;

            // 시작방에서 연결된 비밀방은 절대 보이지 않도록 처리
            if (this.roomData.Type == RoomType.Start && neighbor.roomData.Type == RoomType.Secret && secretRoomVisited == false)
            {
                Color secretColor = neighbor.minimapSpriteRenderer.color;
                secretColor.a = 0f; 
                neighbor.minimapSpriteRenderer.color = secretColor;
            }
           
            else if (neighbor.roomData.Type == RoomType.Secret && secretRoomVisited == false)
            {
                neighbor.minimapSpriteRenderer.enabled = true;
                Color secretColor = neighbor.minimapSpriteRenderer.color;
                secretColor.a = 0f;         
                neighbor.minimapSpriteRenderer.color = secretColor;
            }
            else if (neighbor.roomData.Type == RoomType.Secret && secretRoomVisited == true)
            {
                neighbor.minimapSpriteRenderer.enabled = true;
                Color secretColor = neighbor.minimapSpriteRenderer.color;
                secretColor.a = 0.5f; 
                neighbor.minimapSpriteRenderer.color = secretColor;
            }
            else
            {
                neighbor.minimapSpriteRenderer.enabled = true;
                // 일반 방은 반투명
                Color nextColor = neighbor.minimapSpriteRenderer.color;
                nextColor.a = 0.5f;
                neighbor.minimapSpriteRenderer.color = nextColor;
            }
        }
    }

    


}