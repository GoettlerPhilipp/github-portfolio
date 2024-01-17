using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Drag and Drop System
    [HideInInspector] public Transform parentAfterDrag;
    public Image unitImage;

    public UnitData.UnitType typeOfUnit;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.transform.parent.transform.parent);
        transform.SetAsLastSibling();
        unitImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent((parentAfterDrag));
        transform.position = transform.parent.position;
        unitImage.raycastTarget = true;
    }
}
