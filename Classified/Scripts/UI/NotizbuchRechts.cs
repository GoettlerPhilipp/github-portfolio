using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NotizbuchRechts : MonoBehaviour
{
    public Selectable[] UISelectables;
    private EventSystem eventSystem;
    public List<TMP_InputField> inputFields = new List<TMP_InputField>();
    

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < UISelectables.Length; i++)
            {
                if (UISelectables[i].gameObject == eventSystem.currentSelectedGameObject)
                {
                    UISelectables[(i + 1) % UISelectables.Length].Select();
                    break;
                }
            }
        }
    }

    public void ClearInputFields()
    {
        foreach(TMP_InputField inp in inputFields)
        {
            inp.text = null;
        }
    }
}
