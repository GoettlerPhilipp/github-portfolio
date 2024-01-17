using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningGround : MonoBehaviour
{
    [SerializeField] private ParticleSystem childParticle;
    public float duration;

    private void Awake()
    {
        childParticle = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, childParticle.main.duration);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider _collider)
    {
        if (_collider.CompareTag("Player"))
        {
            _collider.GetComponent<PlayerController>().playerHealth.DamageToPlayer(1);
            Debug.Log("Player Damage");
        }
    }

    private void OnTriggerEnter(Collider _collider)
    {
        
    }
}
