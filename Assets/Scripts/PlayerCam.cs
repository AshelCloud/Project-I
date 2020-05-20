using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public Vector3 offset;

    //맵 경계
    [SerializeField]
    private BoxCollider2D mapBound;

    private Vector3 minBound;
    private Vector3 maxBound;

    //카메라를 맵 밖의 공간으로 내보내지 않기 위해 반너비와 반높이를 더한다.
    private float halfWidth;
    private float halfHeight;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        minBound = mapBound.bounds.min;
        maxBound = mapBound.bounds.max;

        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y) + offset;

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
