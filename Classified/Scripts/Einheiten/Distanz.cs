using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distanz : MonoBehaviour
{

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
