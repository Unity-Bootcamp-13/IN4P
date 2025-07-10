using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossRoomController : RoomController
{
    [SerializeField] GameObject bossAppear;
    public GameObject bossContent;
    public Transform spawnPoint;
    private bool isBossSpawn;

    protected override void Start()
    {
        base.Start();
        minimapSpriteRenderer.color = Color.red;
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
                doorGo.SetActive(false);  // 나중에 열리게 만들 수도 있음
            }
            Door doorComponent = doorGo.GetComponent<Door>();
            doorComponent.type = DoorType.Boss;
            doorComponent.thisDirction = i;
            doorComponent.portalAction += SetNextRoom;
            doorList[i] = doorComponent;
        }
    }

    protected override void GenerateContents()
    {
        if (isBossSpawn)
            return;

        StartCoroutine(C_SpawnBoss());
    }

    public void CheckClearCondition()
    {
        isCleared = true;
        OpenDoors();
        
        Vector3 randomPosition = Random.insideUnitCircle;        
        itemGenerator.GetRandomPickupItem(transform.position + randomPosition * 2f);
        
        int rand = Random.Range(0, 10);
        if (rand <= 9)
        {
            itemGenerator.GetRandomPassiveItem(transform.position);
        }
        else
        {
            itemGenerator.GetRandomActiveItem(transform.position);
        }
    }

    private IEnumerator C_SpawnBoss()
    {
        SoundManager.Instance.PlayBGM(BGM.Boss_Intro);
        var go = Instantiate(bossAppear, transform.position, Quaternion.identity);
        var childGO = go.transform.GetChild(0);

        float t = 1.5f;
        float elapse = 0f;
        Vector3 startPos = childGO.position;
        Vector3 endPos = new Vector3(transform.position.x + 0, transform.position.y - 0.1f, transform.position.z);        

        while (elapse <= t)
        {
            childGO.transform.position = Vector3.Lerp(startPos, endPos, elapse / t);
            elapse += Time.deltaTime;
            yield return null;
        }
        elapse = 0f;
        float shakeDuration = 1.0f;
        while (elapse < shakeDuration)
        {
            float shakePosX = Mathf.PingPong(Time.time * 5f, 0.2f);
            childGO.position = new Vector3(endPos.x + shakePosX, endPos.y, endPos.z);

            elapse += Time.deltaTime;
            yield return null;
        }

        Destroy(go);

        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayBGM(BGM.Boss_Fight);
        GameObject bossGo = Instantiate(bossContent, spawnPoint.position, Quaternion.identity);
        Enemy enemy = bossGo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnDeath += CheckClearCondition;
        }

        isBossSpawn = true;
    }
}