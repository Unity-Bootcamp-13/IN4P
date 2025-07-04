using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("���� ���")]
    public Transform target;

    [Header("������")]
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    public RoomClass room;

    public Tilemap tilePrefab;

    // Ŭ���� ����
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

        // ��ǥ ��ġ
        Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Ŭ����
        //float x = Mathf.Clamp(desired.x, minX, maxX);
        //float y = Mathf.Clamp(desired.y, minY, maxY);
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(desired.x, desired.y, desired.z), ref velocity, smoothTime);
    }

    // �� ��ȯ�� ȣ�� : Ŭ���� ����, ��ġ, �� �ڵ� ����
    public void SetBounds(Tilemap roomBounds)
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;

        // ������ ����
        velocity = Vector3.zero;

        // �� ũ�� World Bounds
        Bounds b = roomBounds.localBounds;
        
        // �ڵ� �� : ���� ī�޶� ����Ʈ���� ������ �� ��ü�� ���̵��� orthographicSize ����
        float mapWidth = b.size.x;
        float mapHeight = b.size.y;
        float screenRatio = (float)Screen.width / Screen.height;

        // �� �ʺ� ���� orthographicSize �� ���� ���� orthographicSize ���
        float sizeBasedOnwidth = (mapWidth * 0.5f) / screenRatio;
        float sizeBasedOnHeight = mapHeight * 0.5f;
        float desiredSize = Mathf.Max(sizeBasedOnwidth, sizeBasedOnHeight);

        cam.orthographicSize = Mathf.Min(defaultSize, desiredSize);

        // Ŭ���� ���� ����
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * screenRatio;

        minX = b.min.x + horzExtent;
        maxX = b.max.x - horzExtent;
        minY = b.min.y + vertExtent;
        maxY = b.max.y - vertExtent;

        // ���� ��  ó�� : ī�޶�
        if (maxX < minX)
        {
            minX = maxX = b.center.x;
        }
        if (maxY < minY)
        {
            minY = maxY = b.center.y;
        }

        // ��� ī�޶� ��ġ ����
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
