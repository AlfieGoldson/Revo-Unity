using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrail : MonoBehaviour
{

    [SerializeField] LineRenderer line;
    [SerializeField] int maxPositions = 50;
    List<Vector3> positions;

    void Awake()
    {
        positions = new List<Vector3>();
    }

    void FixedUpdate()
    {
        if (positions.Count == maxPositions)
        {
            positions.RemoveAt(0);
        }
        var newPos = transform.position;
        newPos.z = 0;
        positions.Add(newPos);

        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());
    }

}
