using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndOfTutorial : MonoBehaviour
{
    public GameObject notizbuch;
    public GameObject endText;

    private void Awake()
    {
        //endText = GetComponent<GameObject>();
    }
    private void Update()
    {

        if(!notizbuch.activeSelf)
        {
            endText.SetActive(true);
            
            
        }
    }
}
