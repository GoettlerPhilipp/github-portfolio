using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitsInventoryBillboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitText;
    [SerializeField] private TextMeshProUGUI currentStateText;
    [SerializeField] private TextMeshProUGUI energyText;
    private Transform mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
    }

    public void UpdateUnitsText(int archer, int swordsman, int horseRider)
    {
        unitText.text = $"Archer: {archer} \nSwordsman: {swordsman} \nHorse Rider: {horseRider}";
    }

    public void UpdateCurrentState(string currentState)
    {
        currentStateText.text = currentState;
    }

    public void UpdateEnergy(float energy)
    {
        energyText.text = $"Energy: {energy:F1}";
    }
}
