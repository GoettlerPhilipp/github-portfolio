using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittingBullet : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed;
    [SerializeField] public float timeBetweenEachShot;
    [HideInInspector] public int bulletHealth;

    private Rigidbody bulletRig;

    private Vector3 direction;

    private bool canShoot = true;

    [SerializeField] private GameObject leftCube;
    [SerializeField] private GameObject rightCube;

    [Header("Bullet Components")] 
    [SerializeField] private GameObject bullet;
    [HideInInspector] public float splittedBulletSpeed;
    [HideInInspector] public int splittedBulletHealth;
    
    private void Awake()
    {
        bulletRig = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = direction.normalized;
        bulletRig.AddForce(direction * (bulletSpeed * Time.deltaTime) * 2, ForceMode.Force);

        Debug.DrawRay(transform.position, transform.right * 10, Color.yellow);
        Debug.DrawRay(transform.position, -transform.right * 10, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.black);
        if (canShoot)
        {
            StartCoroutine(SplitBullet());
        }
    }

    public void ShootOnThisPos(Vector3 _shootPos, float _timeBetweenEachShot, int _bulletHealth, float _splittedBulletSpeed, int _splittedBulletHealth)
    {
        timeBetweenEachShot = _timeBetweenEachShot;
        bulletHealth = _bulletHealth;
        splittedBulletSpeed = _splittedBulletSpeed;
        splittedBulletHealth = _splittedBulletHealth;

        direction = _shootPos - transform.position;
    }

    IEnumerator SplitBullet()
    {
        canShoot = false;
        GameObject leftBullet = Instantiate(bullet, transform.position, Quaternion.identity, transform);
        GameObject rightBullet = Instantiate(bullet, transform.position, Quaternion.identity, transform);
        leftBullet.transform.localScale = new Vector3(1, 1, 1);
        rightBullet.transform.localScale = new Vector3(1, 1, 1);
        leftBullet.GetComponent<Bullet>().bulletSpeed = splittedBulletSpeed;
        rightBullet.GetComponent<Bullet>().bulletSpeed = splittedBulletSpeed;
        leftBullet.GetComponent<Bullet>().ShootOnThisPos(leftCube.transform.position,transform.position.y,splittedBulletHealth);
        rightBullet.GetComponent<Bullet>().ShootOnThisPos(rightCube.transform.position,transform.position.y,splittedBulletHealth);
        yield return new WaitForSecondsRealtime(timeBetweenEachShot);
        canShoot = true;
    }
}
