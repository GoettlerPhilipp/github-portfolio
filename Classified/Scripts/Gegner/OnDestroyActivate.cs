using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OnDestroyActivate : MonoBehaviour
{
    [SerializeField] GameObject playerLand;
    GameObject ownLand;

    private void Awake()
    {
        ownLand = this.gameObject;
    }

    private void OnDestroy()
    {
        if (playerLand == null)
            return;
        else if(playerLand != null)
            this.playerLand.SetActive(true);
    }

}
