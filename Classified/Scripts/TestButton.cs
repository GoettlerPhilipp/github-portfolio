using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    GameObject button;

    public static bool isPaused;

    CheckUnits checkUnits;
    CreateUnits createUnits;

    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject;
        checkUnits = GetComponent<CheckUnits>();
        createUnits = GetComponent<CreateUnits>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                unityEvent.Invoke();
            }

            isPaused = true;
        }
        isPaused = false;


        
    }

    public void SetSpawnPosition()
    {
        CreateUnits.Instance.spawnPosition = this.transform;
    }

    

    public void NumberOfUnits()
    {
        CreateUnits.Instance.soldierNumber = this.checkUnits.soldier;
        CreateUnits.Instance.tankNumber = this.checkUnits.tank;
        CreateUnits.Instance.planeNumber = this.checkUnits.plane;
        CreateUnits.Instance.flakNumber = this.checkUnits.flak;
    }

    
    
}
