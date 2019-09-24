using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrail : MonoBehaviour
{

    [SerializeField] LineRenderer line;
    [SerializeField] int maxPositions = 50;
    List<Vector3> positions;

    bool disabled = false;

    void Awake()
    {
        positions = new List<Vector3>();
    }

    void FixedUpdate()
    {
        if (disabled)
        {
            if (positions.Count == 0)
                Destroy(gameObject);
            else
                positions.RemoveAt(0);
        }
        else
        {
            if (positions.Count == maxPositions)
            {
                positions.RemoveAt(0);
            }
            var newPos = transform.position;
            newPos.z = 0;
            if (positions.Count >= 2)
                positions[positions.Count - 1] = Vector3.Lerp(positions[positions.Count - 2], newPos, 0.5f);
            positions.Add(newPos);
        }

        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());
    }

    public void DisableTrail()
    {
        disabled = true;
    }

}
