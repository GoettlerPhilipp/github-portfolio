using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CurrentUnitsInQueue : MonoBehaviour
{
    //responsible for filling effects on the queue
    
    
    public List<BuildUnits.UnitTypeRoundsCount> queueGameObjectsImg;
    public List<GameObject> queueGameObject;
    public GameObject currentUnit;

    public BuildUnits currentBuilding;
    
    [Header("Progress Bar")]
    public Image progressBar;
    public float duration;
    private Color tempColorInactive;

    private void Start()
    {
        queueGameObjectsImg = new List<BuildUnits.UnitTypeRoundsCount>();
        tempColorInactive.r = 1f;
        tempColorInactive.g = 1f;
        tempColorInactive.b = 1f;
        tempColorInactive.a = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBuilding != null)
        {
            if (currentBuilding.nextUnitInProgress.unit != null)
            {
                currentUnit.GetComponent<Image>().sprite = currentBuilding.nextUnitInProgress.image;
                var tempColor = currentUnit.GetComponent<Image>().color;
                tempColor.a = 1f;
                currentUnit.GetComponent<Image>().color = tempColor;
            }
            else if (currentBuilding.nextUnitInProgress.unit == null)
            {
                currentUnit.GetComponent<Image>().sprite = null;
                currentUnit.GetComponent<Image>().color = tempColorInactive;
            }

            queueGameObjectsImg = currentBuilding.queueOfUnits.ToList();
            for (int i = 0; i < queueGameObjectsImg.Count; i++)
            {
                queueGameObject[i].GetComponent<Image>().sprite = queueGameObjectsImg[i].image;
                var tempColor = queueGameObject[i].GetComponent<Image>().color;
                tempColor.a = 1f;
                queueGameObject[i].GetComponent<Image>().color = tempColor;
            }
            
            CheckForList();
            if (currentBuilding.nextUnitInProgress.timer >= 0)
            {
                progressBar.fillAmount = Mathf.InverseLerp(0, duration, currentBuilding.nextUnitInProgress.timer);
            
            }
        }
    }
    
    void CheckForList()
    {
        switch (queueGameObjectsImg.Count)
        {
            case 0:
                queueGameObject[0].GetComponent<Image>().sprite = null;
                queueGameObject[0].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[1].GetComponent<Image>().sprite = null;
                queueGameObject[1].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[2].GetComponent<Image>().sprite = null;
                queueGameObject[2].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[3].GetComponent<Image>().sprite = null;
                queueGameObject[3].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[4].GetComponent<Image>().sprite = null;
                queueGameObject[4].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[5].GetComponent<Image>().sprite = null;
                queueGameObject[5].GetComponent<Image>().color = tempColorInactive;
                break;
            case 1:
                queueGameObject[1].GetComponent<Image>().sprite = null;
                queueGameObject[1].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[2].GetComponent<Image>().sprite = null;
                queueGameObject[2].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[3].GetComponent<Image>().sprite = null;
                queueGameObject[3].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[4].GetComponent<Image>().sprite = null;
                queueGameObject[4].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[5].GetComponent<Image>().sprite = null;
                queueGameObject[5].GetComponent<Image>().color = tempColorInactive;
                break;
            case 2:
                queueGameObject[2].GetComponent<Image>().sprite = null;
                queueGameObject[2].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[3].GetComponent<Image>().sprite = null;
                queueGameObject[3].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[4].GetComponent<Image>().sprite = null;
                queueGameObject[4].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[5].GetComponent<Image>().sprite = null;
                queueGameObject[5].GetComponent<Image>().color = tempColorInactive;
                break;
            case 3:
                queueGameObject[3].GetComponent<Image>().sprite = null;
                queueGameObject[3].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[4].GetComponent<Image>().sprite = null;
                queueGameObject[4].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[5].GetComponent<Image>().sprite = null;
                queueGameObject[5].GetComponent<Image>().color = tempColorInactive;
                break;
            case 4:
                queueGameObject[4].GetComponent<Image>().sprite = null;
                queueGameObject[4].GetComponent<Image>().color = tempColorInactive;
                queueGameObject[5].GetComponent<Image>().sprite = null;
                queueGameObject[5].GetComponent<Image>().color = tempColorInactive;
                break;
            case 5:
                queueGameObject[5].GetComponent<Image>().sprite = null;
                queueGameObject[5].GetComponent<Image>().color = tempColorInactive;
                break;
        }
    }
}
