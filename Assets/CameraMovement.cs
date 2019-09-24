using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] Transform[] targets;

    [SerializeField] Vector3 offset;

    [SerializeField]
    float
        followSpeed = 6f,
        zoomSpeed = 8f,
        minZoom = 8,
        maxZoom = 4;

    Bounds bounds;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Length == 0)
            return;

        SetBounds();

        Move();
        Zoom();
    }

    Bounds SetBounds()
    {
        bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 1; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Length == 1)
            return targets[0].position;

        return bounds.center;
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * followSpeed);
    }

    void Zoom()
    {
        float distance = GetGreatestDistance();

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Mathf.Clamp(distance, maxZoom, minZoom), Time.deltaTime * zoomSpeed);
    }

    float GetGreatestDistance()
    {
        return bounds.size.x / 2;
    }
}
