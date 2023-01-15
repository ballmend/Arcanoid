using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public GameObject plane;
    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Ball"))
        {

        }

    }
}
