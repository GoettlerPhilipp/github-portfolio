using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    
    public void doExitGame()
    {
        Application.Quit();
        Debug.Log("Spiel Geschlossen");
    }
}
