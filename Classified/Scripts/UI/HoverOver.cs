using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOver : MonoBehaviour
{
    public GameObject unit;


    private void OnMouseOver()
    {

        unit.SetActive(true);

        Debug.Log("Drüber");
    }

    private void OnMouseExit()
    {
        Debug.Log("Raus");
        unit.SetActive(false);
    }
}
