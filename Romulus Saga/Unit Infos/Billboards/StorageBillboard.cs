using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageBillboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventory;
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

    public void UpdateText(int wood, int stone, int food)
    {
        inventory.text = $"Wood: {wood} \nStone: {stone} \nFood: {food}";
    }
}
