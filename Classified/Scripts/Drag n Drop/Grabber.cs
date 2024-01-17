using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private GameObject selectedObject;
    private Rigidbody rigidbody;
    Vector3 originalPos;
    Vector3 startPos;

    [SerializeField] private Texture2D openHand;
    [SerializeField] private Texture2D closedHand;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        //originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        originalPos = gameObject.transform.position;
        startPos = gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            if(selectedObject == null)                          // Um ein Objekt aufheben zu können.
            {
                originalPos = gameObject.transform.position;
                RaycastHit hit = CastRay();

                if(hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))       // Checkt ob das GameObject einen "drag"-Tag besitzt.
                    {
                        return;
                    }
                    rigidbody.isKinematic = true;
                    selectedObject = hit.collider.gameObject;
                    Cursor.SetCursor(closedHand, Vector2.zero, CursorMode.ForceSoftware);
                    //Cursor.visible = false;
                }
            }else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

                rigidbody.isKinematic = false;
                selectedObject = null;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                //Cursor.visible = true;
            }
        }
        if(selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, -1f);
        }


    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    private void ResetPosi()
    {
        transform.position = startPos;
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(openHand, Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
