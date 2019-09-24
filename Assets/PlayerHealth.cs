using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] float healthPercent = 0;

    public void TakeDamage(float damage)
    {
        healthPercent += damage;
    }

}

class StatLine
{
    public float
        strength = 1f,
        armor = 1f,
        mobility = 1f,
        range = 1f;
}