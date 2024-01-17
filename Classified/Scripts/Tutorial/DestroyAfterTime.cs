using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public int timer;

    private void Destroy()
    {
        Destroy(gameObject);
    }
    private void Awake()
    {
        Invoke("Destroy", timer);
    }
}
