using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notizbuch : MonoBehaviour
{

    public GameObject buch;

    
    public void Test()
    {
        if(buch.activeSelf == true)
        {
            
            buch.SetActive(false);
        }
        else if(buch.activeSelf == false)
        {
            
            buch.SetActive(true);
        }
    }
}
