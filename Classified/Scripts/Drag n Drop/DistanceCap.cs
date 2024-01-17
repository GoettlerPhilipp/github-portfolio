using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class DistanceCap : MonoBehaviour
{
    Vector3 borderPos;
    public float xleft, xright, ydown, yup;

    private void Awake()
    {
        borderPos = gameObject.transform.position;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, borderPos.x - xleft, borderPos.x + xright);              //X-Achsen Bereich sperre
        pos.y = Mathf.Clamp(pos.y, borderPos.y - ydown, borderPos.y + yup);                 //Y-Achsen Bereich sperre
        transform.position = pos;
        borderPos.z = Mathf.Clamp(transform.position.z, transform.position.z, transform.position.z);

    }
    
}
