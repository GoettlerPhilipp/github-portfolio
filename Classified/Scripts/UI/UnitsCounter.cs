using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitsCounter : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI soldierText;
    public TextMeshProUGUI tankText;
    public TextMeshProUGUI planeText;
    public TextMeshProUGUI flakText;
    [Header("Images")]
    public Image soldierImg;
    public Image tankImg;
    public Image planeImg;
    public Image flakImg;


    CreateUnits numbers;



    private void Update()
    {
        numbers = GetComponent<CreateUnits>();
        
    }
    private void Awake()
    {
        soldierText.SetText(numbers.soldierNumber.ToString());
        tankText.SetText(numbers.tankNumber.ToString());
        planeText.SetText(numbers.planeNumber.ToString());
        flakText.SetText(numbers.flakNumber.ToString());
    }
}
