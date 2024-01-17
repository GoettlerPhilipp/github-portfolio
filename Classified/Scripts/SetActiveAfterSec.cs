using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterSec : MonoBehaviour
{
    public int waitSec;
    public void SetActive()
    {
        Invoke("Wait", waitSec * Time.deltaTime);
    }

    void Wait()
    {
        gameObject.SetActive(true);
    }
}
