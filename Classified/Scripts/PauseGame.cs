using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    float previousTimeScale = 1;

    public static bool isPause;
    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else if( Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
        }
    }

    public void PauseTheGame(bool pause)
    {
        pause = false;
        if(pause == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        return;
    }
    public void StartTheGame()
    {
        Time.timeScale = 1;
    }
}
