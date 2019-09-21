using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour
{

    public float gravity = -9.8f;

    public void Attract(Rigidbody2D body, float gravityMultiplier)
    {
        Vector3 gravityUp = (body.transform.position - transform.position).normalized;
        Vector3 localUp = body.transform.up;

        body.AddForce(gravityUp * gravity * gravityMultiplier);
        body.transform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.transform.rotation;
    }
}