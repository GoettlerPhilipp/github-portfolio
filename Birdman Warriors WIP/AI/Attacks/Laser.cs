using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Laser : MonoBehaviour
{
    [SerializeField] private Vector3 startAngle;
    [SerializeField] private Vector3 endAngle;

    public int laserDamage;

    private Quaternion startRot, endRot;

    [SerializeField] private float interp = 0;
    public float speed = 1f;

    private ParticleSystem thisParticleSystem;


    public bool checkIfLaserStarted;
    [SerializeField] private float widthOfRay;

    private void Awake()
    {
        thisParticleSystem = gameObject.GetComponent<ParticleSystem>();
        thisParticleSystem.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        startRot = Quaternion.Euler(startAngle);
        endRot = Quaternion.Euler(endAngle);
        //transform.rotation = startRot;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkIfLaserStarted)
            StartCoroutine(StartLaser());
    }

    public void DrawRay()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(this.transform.position, transform.forward * 50, out hitInfo)||
            Physics.Raycast(new Vector3(transform.position.x - widthOfRay, transform.position.y, transform.position.z), transform.forward * 50, out hitInfo)||
            Physics.Raycast(new Vector3(transform.position.x + widthOfRay, transform.position.y, transform.position.z), transform.forward * 50, out hitInfo))
        {
            if(hitInfo.collider.CompareTag("Player"))
                PlayerController.instance.playerHealth.DamageToPlayer(laserDamage);
        }
    }

    public IEnumerator StartLaser()
    {
        if (interp < 1) interp += Time.deltaTime * speed;
        transform.rotation = Quaternion.Lerp(startRot, endRot, interp);
        DrawRay();
        Debug.DrawRay(this.transform.position, transform.forward * 50, Color.cyan);
        Debug.DrawRay(new Vector3(transform.position.x - widthOfRay, transform.position.y, transform.position.z) , transform.forward * 50, Color.cyan);
        Debug.DrawRay(new Vector3(transform.position.x + widthOfRay, transform.position.y, transform.position.z) , transform.forward * 50, Color.cyan);

        if (transform.rotation == endRot)
        {
            thisParticleSystem.Stop();
            yield return new WaitForSecondsRealtime(2f);
            checkIfLaserStarted = false;
            ResetLaser();
        }
    }

    public void ResetLaser()
    {
        transform.rotation = startRot;
        interp = 0;
    }
}
