using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("추적 대상")]
    public Transform target;

    [Header("스무딩")]
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    public RoomClass room;

    public Tilemap tilePrefab;

    // 클램프 범위
    private float minX, maxX, minY, maxY;

    private float defaultSize = 5;
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        if (!cam.orthographic)
        {
            cam.orthographic = true;
        }
    }

    private void Start()
    {
        if (target == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) target = p.transform;
        }
        //SetBounds(tilePrefab);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraController: target is null.");
            return;
        }

        // 목표 위치
        Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 클램핑
        //float x = Mathf.Clamp(desired.x, minX, maxX);
        //float y = Mathf.Clamp(desired.y, minY, maxY);
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(desired.x, desired.y, desired.z), ref velocity, smoothTime);
    }

    // 방 전환시 호출 : 클램프 범위, 위치, 줌 자동 조절
    public void SetBounds(Tilemap roomBounds)
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;

        // 스무딩 중지
        velocity = Vector3.zero;

        // 방 크기 World Bounds
        Bounds b = roomBounds.localBounds;
        
        // 자동 줌 : 방이 카메라 뷰포트보다 작으면 방 전체가 보이도록 orthographicSize 조절
        float mapWidth = b.size.x;
        float mapHeight = b.size.y;
        float screenRatio = (float)Screen.width / Screen.height;

        // 맵 너비 기준 orthographicSize 및 높이 기준 orthographicSize 계산
        float sizeBasedOnwidth = (mapWidth * 0.5f) / screenRatio;
        float sizeBasedOnHeight = mapHeight * 0.5f;
        float desiredSize = Mathf.Max(sizeBasedOnwidth, sizeBasedOnHeight);

        cam.orthographicSize = Mathf.Min(defaultSize, desiredSize);

        // 클램핑 범위 재계산
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * screenRatio;

        minX = b.min.x + horzExtent;
        maxX = b.max.x - horzExtent;
        minY = b.min.y + vertExtent;
        maxY = b.max.y - vertExtent;

        // 작은 방  처리 : 카메라
        if (maxX < minX)
        {
            minX = maxX = b.center.x;
        }
        if (maxY < minY)
        {
            minY = maxY = b.center.y;
        }

        // 즉시 카메라 위치 갱신
        JumpToTarget();

    }

    private void JumpToTarget()
    {
        if (target == null) return;
        float z = transform.position.z;
        float x = Mathf.Clamp(target.position.x, minX, maxX);
        float y = Mathf.Clamp(target.position.y, minY, maxY);
        transform.position = new Vector3(x, y, z);
    }
}
