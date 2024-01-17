using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpielerGebiet : MonoBehaviour
{
    [SerializeField] GameObject gegnerGebiet;
    SpriteRenderer spriteRenderer;
    BoxCollider boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        //gameObject = GetComponent<SpriteRenderer>();
        
    }
    private void Update()
    {
        if(gegnerGebiet == null)
        {
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
            transform.gameObject.SetActive(true);
        }
    }
}
