using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Sieg : MonoBehaviour
{
    [SerializeField] string endeText;
    bool onetime = false;
    [SerializeField] string scene;

    private void Update()
    {
        if (!onetime)
        {

            if (transform.childCount == 0)
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
        
    }


    
}
