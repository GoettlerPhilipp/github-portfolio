using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgMove : MonoBehaviour
{
    public GameObject msg;
    public GameObject trueTest;


    public void Activate()
    {
        if (msg == true)
        {
            trueTest.SetActive(true);
        }
    }
}
