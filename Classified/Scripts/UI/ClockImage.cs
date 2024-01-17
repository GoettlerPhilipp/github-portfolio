using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClockImage : MonoBehaviour
{
    [Header("Image")]
    Image myRenderer;
    [SerializeField] Sprite[] clockImage;

    RoundManager roundManager;

    private void Awake()
    {
        myRenderer = GetComponent<Image>();
        roundManager = GetComponent<RoundManager>();
    }

    private void Update()
    {
        ChangeImg();
    }
    void ChangeImg()
    {

        
        if (RoundManager.Instance.roundPerTurn == 4)
        {
            myRenderer.sprite = clockImage[0];
        }
        else if (RoundManager.Instance.roundPerTurn == 3)
        {
            myRenderer.sprite = clockImage[1];
        }
        else if (RoundManager.Instance.roundPerTurn == 2)
        {
            myRenderer.sprite = clockImage[2];
        }
        else if (RoundManager.Instance.roundPerTurn == 1)
        {
            myRenderer.sprite = clockImage[3];
        }
        else if (RoundManager.Instance.roundPerTurn == 0)
        {
            myRenderer.sprite = clockImage[4];
        }
    }
}
