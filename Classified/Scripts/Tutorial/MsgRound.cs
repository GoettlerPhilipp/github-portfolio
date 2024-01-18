using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgRound : MonoBehaviour
{
    public GameObject msg;
    public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if(RoundManager.Instance.roundPerTurn == 0)
        {
            if(msg != null)
                msg.SetActive(true);
            button.GetComponent<Button>().enabled = true;
        }
    }

    
}
