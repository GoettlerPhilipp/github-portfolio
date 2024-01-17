using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void DestroyAfterSec()
    {
        Invoke("Destroy", 5.0f * Time.deltaTime);
    }

    
}
