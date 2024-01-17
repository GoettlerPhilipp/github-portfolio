using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AI_EnemyBaseBillboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private TextMeshProUGUI bestActionText;
    [SerializeField] private TextMeshProUGUI unitsText;
    private Transform mainCameraTransform;
    

    private void Awake()
    {
        mainCameraTransform = Camera.main.transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }

    public void UpdateInventoryText(int wood, int stone, int food)
    {
        inventoryText.text = $"Wood: {wood} \nStone: {stone} \nFood: {food}";
    }

    public void UpdateBestAction(string bestAction)
    {
        bestActionText.text = bestAction;
    }

    public void UpdateUnitsText(int archer, int swordsman, int horseRider)
    {
        unitsText.text = $"Archer: {archer} \nSwordsman: {swordsman} \nHorse Rider: {horseRider}";
    }
}
