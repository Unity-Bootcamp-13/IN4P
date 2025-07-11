using System.Collections;
using UnityEngine;

public abstract class RoomController : MonoBehaviour
{
    [SerializeField] ItemServiceSO itemServiceSO;
    protected ItemGenerator itemGenerator;

    private Camera mainCamera;
    public Transform[] doorSpawnPoints = new Transform[4];
    public GameObject[] doorPrefabs;
    protected Door[] doorList = new Door[4];
    
    public GameObject minimapObject;
    protected SpriteRenderer minimapSpriteRenderer;

    private Room roomData;
    public RoomController[] nextRoomControllers = new RoomController[4];

    public bool secretRoomVisited;
    public bool treasureRoomVisited;
    public bool isCleared;
    public static bool isStartroom;


    private void Awake()
    {
        mainCamera = Camera.main;
        GameObject minimapGo = Instantiate(minimapObject, transform.position + Vector3.up * 200, Quaternion.identity, transform);
        minimapSpriteRenderer = minimapGo.GetComponent<SpriteRenderer>();
        minimapSpriteRenderer.enabled = false;
    }

    protected virtual void Start()
    {
        itemGenerator = new ItemGenerator(itemServiceSO.itemService);
        OpenDoors();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            OpenDoors();
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

        if (nextRoom.roomData.Type == RoomType.Secret)
        {
            nextRoom.CloseDoors();
            int oppositeDir = (direction + 2) % 4;
            nextRoom.doorList[oppositeDir].gameObject.GetComponent<SecretDoorController>().OpenSecretDoor();
            
            if(!nextRoom.secretRoomVisited)
            {
                nextRoom.GenerateContents();
                nextRoom.secretRoomVisited = true;
            }
        }

        if(nextRoom.roomData.Type == RoomType.Treasure && nextRoom.treasureRoomVisited == false)
        {
            nextRoom.GenerateContents();
            nextRoom.treasureRoomVisited = true;
        }

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

    protected void SetMinimap()
{
    minimapSpriteRenderer.enabled = true;
    Color currentColor = minimapSpriteRenderer.color;
    currentColor.a = 1f;
    minimapSpriteRenderer.color = currentColor;

    foreach (RoomController neighbor in nextRoomControllers)
    {
        if (neighbor == null) continue;

        RoomType type = neighbor.roomData.Type;
        SpriteRenderer renderer = neighbor.minimapSpriteRenderer;

        if (this.roomData.Type == RoomType.Start && type == RoomType.Secret && !secretRoomVisited)
        {
            renderer.enabled = false;
            continue;
        }

        renderer.enabled = true;
        Color color = renderer.color;

        if (type == RoomType.Secret)
        {
            color.a = secretRoomVisited ? 0.5f : 0f;
        }
        else
        {
            color.a = 0.5f;
        }

        renderer.color = color;
    }
}

    


}