using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AI_Billboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI bestActionText;
    [SerializeField] private TextMeshProUGUI inventoryText;
    private Transform mainCameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }

    public void UpdateStatsText(float energy)
    {
        statsText.text = $"Energy: {energy:F1}";
    }

    public void UpdateFSM(string _fsm)
    {
        bestActionText.text = _fsm;
    }

    public void UpdateInventoryText(int wood, int stone, int food)
    {
        inventoryText.text = $"Wood: {wood}\nStone: {stone}\nFood: {food}";
    }
}
