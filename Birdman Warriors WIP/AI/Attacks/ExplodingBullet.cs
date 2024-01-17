using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : MonoBehaviour
{
    private Vector3 distance;
    
    [SerializeField] public float bulletSpeed;
    
    private Vector3 bombPos;

    public float explotionTimer;

    private bool explotionStarted = false;

    public GameObject bulletPoint;
    private int bulletsToSpawn;
    private float normalBulletbulletSpeed;

    private Color startColor;


    private void Awake()
    {
        startColor = GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        distance = bombPos - gameObject.transform.position;
        if (distance.magnitude > 0.2f)
        {
            Vector3 velocity = distance.normalized * bulletSpeed * Time.deltaTime;
            this.transform.position = this.transform.position + velocity;
        }
        else if(distance.magnitude <= 0.2f && !explotionStarted)
        {
            Debug.Log("Start Timer");
            
            StartCoroutine(StartExplotion());
            explotionStarted = true;
        }
        /*
        distance = bombPos - gameObject.transform.position;
        Debug.Log("Distance Magnitude: " + distance.magnitude);
        if (distance.magnitude > 1f)
        {
            distance = distance.normalized;
            bulletRig.AddForce(distance * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            bulletRig.isKinematic = true;
        }*/

        //distance = distance.normalized;
        //bulletRig.AddForce(distance * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Cover"))
        {
            //bombPos = this.transform.position;
            //Debug.Log("Distance: " + distance);
            //
            _collider.GetComponent<CoverHealth>().GetDamage();
            gameObject.SetActive(false);
        }
    }

    public void ExplodingPos(int _howManyBulletsSpawn, float _bulletSpeed, Vector3 _shootPos)
    {
        BulletsToLoad.instance.explosivBulletsQueue.Enqueue(gameObject);
        bombPos = _shootPos;
        bulletsToSpawn = _howManyBulletsSpawn;
        distance = bombPos - gameObject.transform.position;
        normalBulletbulletSpeed = _bulletSpeed;
    }

    public IEnumerator StartExplotion()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSecondsRealtime(explotionTimer / 4);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        yield return new WaitForSecondsRealtime(explotionTimer / 4);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSecondsRealtime(explotionTimer / 4);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        yield return new WaitForSecondsRealtime(explotionTimer / 4);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        
        CreateShootingPointsAroundMe(bulletsToSpawn, normalBulletbulletSpeed);
        gameObject.SetActive(false);
        GetComponent<MeshRenderer>().material.color = startColor;
    }

    public void CreateShootingPointsAroundMe(int num, float _bulletSpeed)
    {
        for (int i = 0; i < num; i++)
        {
            float radians = 2 * Mathf.PI / num * i;

            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            var spawnPos = this.transform.position + spawnDir * 5;

            GameObject spawnBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
            spawnBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            spawnBullet.SetActive(true);
            spawnBullet.transform.position = transform.position;
            spawnBullet.GetComponent<Bullet>().bulletSpeed = _bulletSpeed;
            spawnBullet.GetComponent<Bullet>().ShootOnThisPos(spawnPos, transform.position.y, 1);
        }
    }
    
}
