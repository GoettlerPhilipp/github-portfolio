using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    

    public void ChangeScene(string _levelname)
    {
        SceneManager.LoadScene(_levelname, LoadSceneMode.Single);
    }
}
