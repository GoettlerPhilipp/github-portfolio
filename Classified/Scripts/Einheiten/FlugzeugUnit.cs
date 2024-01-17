using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlugzeugUnit : MonoBehaviour
{
    
    void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }

    void ResetPos()
    {
        gameObject.SendMessageUpwards("ResetPosi");
    }
}
