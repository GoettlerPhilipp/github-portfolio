using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tipToShow;

    public GameObject unit;

    private float timeToWait = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        InfoField.OnMouseLoseFocus();
    }

    private void ShowMessage()
    {
        //InfoField.OnMouseHover(tipToShow, Input.mousePosition);
        InfoField.OnMouseEnter(unit);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
