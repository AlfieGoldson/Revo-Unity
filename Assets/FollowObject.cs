using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{

    public Transform obj;
    public float lerpSpeed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, obj.position, Time.deltaTime * lerpSpeed);
    }

}
