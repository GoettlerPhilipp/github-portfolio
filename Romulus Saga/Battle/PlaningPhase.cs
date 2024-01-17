using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaningPhase : MonoBehaviour
{
    //Dafür da, in der Planing Phase Einheiten umzustationieren und Einheiten aufzuteilen
    [Header("Einheiten Bewegen")]
    public Camera battleCamera;
    public GameObject pickedGameObject;
    public Vector3 characterPos;
    
    [Header("Einheiten Aufteilen")]
    public GameObject transferSlider;
    public GameObject oldUnit;
    public GameObject newUnit;
    public PlaningPhaseSlider sliderForUnitTransfer;

    private void Start()
    {
        battleCamera = GameObject.Find("BattleCamera").GetComponent<Camera>();
        transferSlider = GameObject.Find("BattleSliderBG");
    }

    private void Update()
    {
        Debug.DrawRay(battleCamera.transform.position, Input.mousePosition, Color.black);
        if(BattleRoundManager.instance.currentState == BattleStates.PlaningPhase)
            PickUnitUp();
    }

    void PickUnitUp()
    {

        RaycastHit hit;
        Ray ray = battleCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 pickedGameObjectPos;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Unit"))
                {
                    pickedGameObject = hit.transform.gameObject;
                    characterPos = pickedGameObject.transform.position;
                }
            }

        }

        if (pickedGameObject != null)
        {
            pickedGameObjectPos = new Vector3(pickedGameObject.transform.position.x,
                pickedGameObject.transform.position.y + 0.1f, pickedGameObject.transform.position.z);
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    pickedGameObject.transform.position = hit.transform.position;
                }

                if (Input.GetMouseButtonDown(1) && pickedGameObject.GetComponent<UnitCombat>().numberOfUnits > 1)
                {
                    oldUnit = pickedGameObject;
                    newUnit = Instantiate(pickedGameObject, pickedGameObject.transform.position, Quaternion.identity);
                    newUnit.GetComponent<UnitCombat>().numberOfUnits = 0;
                    transferSlider.GetComponent<Image>().enabled = true;
                    for (int i = 0; i < transferSlider.transform.childCount; i++)
                        transferSlider.transform.GetChild(i).gameObject.SetActive(true);
                    sliderForUnitTransfer.thisSlider.maxValue = oldUnit.GetComponent<UnitCombat>().numberOfUnits;
                    sliderForUnitTransfer.thisSlider.value = sliderForUnitTransfer.thisSlider.maxValue;
                    sliderForUnitTransfer.rightNumberUnits.text = 0.ToString();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(pickedGameObjectPos, -transform.up, out hit))
                {
                    if (hit.transform.gameObject.GetComponent<HexaTile>().isOccupied)
                    {
                        pickedGameObject.transform.position = characterPos;
                    }
                    else
                        hit.transform.gameObject.GetComponent<HexaTile>().isOccupied = true;
                }
                pickedGameObject = null;
            }
        }
    }

}
