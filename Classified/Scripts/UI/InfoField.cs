using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class InfoField : MonoBehaviour
{
    [SerializeField] private GameObject gameObject1;

    public static Action OnMouseLoseFocus;

    public static Action<GameObject> OnMouseEnter;

    private void OnEnable()
    {
        
        OnMouseLoseFocus += HideTip;
        OnMouseEnter += ShowImage;
    }

    private void OnDisable()
    {
        
        OnMouseLoseFocus -= HideTip;
        
    }
    private void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        gameObject1.SetActive(true);
    }

    private void HideTip()
    {
        gameObject1.SetActive(false);
    }
    
    private void ShowImage(GameObject go)
    {
        go.SetActive(true);
    }
}
