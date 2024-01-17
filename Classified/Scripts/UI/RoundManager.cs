using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance {get; private set;}
    [Header("Rounds")]      //Alle leicht veränderbar im Inspector -> Balancing
    public int roundPerTurn;        //Züge pro Runde
    public int addToTurns;          //Züge hinzufügen           
    public int capTurn;             //Maximale anzahl an Zügen pro Runde

    CheckUnits checkUnits;

    [Header("End Game")]
    public int maxTurn;           //Leicht veränderbare Maximale Rundenanzahl
    public string sceneName;

    [Header("UI")]
   
    public TextMeshProUGUI roundCountDown;

    private void Awake()
    {
        checkUnits = GetComponent<CheckUnits>();
        Instance = this;
    }

    public void TurnOver()
    {
        roundPerTurn += addToTurns;
        maxTurn -= 1;
    }
    private void Update()
    {
        
        if (roundPerTurn >= capTurn)
                roundPerTurn = capTurn;
        
        roundCountDown.SetText(maxTurn.ToString());
        ChangeScene(sceneName);
    }

    public void ChangeScene(string _levelname)
    {
        if(maxTurn <= 0)
            SceneManager.LoadScene(_levelname, LoadSceneMode.Single);
    }
}
