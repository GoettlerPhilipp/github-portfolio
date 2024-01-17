using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class CreateAttack : MonoBehaviour
{
    //Skript für die verschiedenen Angriffs Pattern
    [Header("Bullet")]
    [SerializeField] public GameObject bullet;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject splittingBullet;
    private GameObject spawnedBullet;
    [SerializeField] private int multiShotBulletCount;
    [SerializeField] private float shotsPosNextToPlayer;
    [SerializeField] private float coneAttackSecondsBetweenEachShot;
    [SerializeField] public int bulletHealth;
    [SerializeField] private Vector2 minAndMaxBullets;
    [SerializeField] private GameObject VFormationGo;
    
    [Header("Cloud")]
    [SerializeField] private GameObject cloud;
    private GameObject spawnedCloud;

    [Header("Laser")] 
    [SerializeField] public GameObject rightLaser;
    [SerializeField] public GameObject leftLaser;

    [Header("Explotion")] 
    [SerializeField] private GameObject explodingBullet;
    [HideInInspector] public bool explotionCooldown = false;

    [Header("Comet")] 
    [SerializeField] private GameObject comet;
    [SerializeField] private GameObject cometShootPos;
    
    [Header("Positions")]
    public GameObject leftCube;
    public GameObject rightCube;
    private Vector3 playerPos;
    [SerializeField] public GameObject bulletSpawnPosGameObject;
    

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (Input.GetKeyDown(KeyCode.V))
        { 
            //SpawnFollowTargetBullet(20);
            //SpawnSplittingBullet(50, 0.5f, 3, 75, bulletHealth);
            //SpawnVFormationAroundMe(50, 1f, 0.2f, Random.Range(3,6),  3);
            //SpawnBulletsInVFormation(100, 1f, 0.1f, Random.Range(1, 5), Random.Range(2, 5));
            //SpawnTurret(10);
            //SpawnFeint(100, 2f, minAndMaxBullets);
            //SpawnBulletsInSpirale(100, 10, 4);
            //SpawnFollowTargetBullet(25);
            //SpawnBulletsAroundMe(1, 0.2f ,100, 50f);
            //MultiAttack(7, 100, 2);
            //SpawnComet();
            SpawnSplittingExplodingBullet(30, 10, 50);
        }
    }

    #region BasicAttack

    //Funktion um die Kugeln spawnen zu lassen
    public void SpawnBullet(float _speed)
    {
        Vector3 bulletSpawnPos = new Vector3(transform.position.x, bulletSpawnPosGameObject.transform.position.y,
            transform.position.z);
        Vector3 shootPos = new Vector3(playerPos.x + Random.Range(-5 , 5), bulletSpawnPos.y, playerPos.z + Random.Range(-5, 5));
        GameObject newBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
        newBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        newBullet.transform.position = bulletSpawnPos;
        newBullet.GetComponent<Bullet>().bulletSpeed = _speed;
        newBullet.SetActive(true);
        newBullet.GetComponent<Bullet>().ShootOnPlayer(bulletHealth, false, shootPos);
    }

    public void SpawnFollowTargetBullet(float _speed)
    {
        Vector3 bulletSpawnPos = new Vector3(transform.position.x, bulletSpawnPosGameObject.transform.position.y,
            transform.position.z);
        Vector3 shootPos = new Vector3(playerPos.x, bulletSpawnPos.y, playerPos.z);
        GameObject newBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
        newBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        newBullet.transform.position = bulletSpawnPos;
        newBullet.GetComponent<Bullet>().bulletSpeed = _speed;
        newBullet.SetActive(true);
        newBullet.GetComponent<Bullet>().ShootOnPlayer(bulletHealth, true, shootPos);
    }
    public void MultiAttack(int _bulletCount, float _speed, float _shotPosNextToPlayer)
    {
        Vector3 bulletSpawnPos = transform.position;
        bulletSpawnPos.y = bulletSpawnPosGameObject.transform.position.y;
        
        float evenOrOdd = (float)_bulletCount % 2;
        float distanceBetweenEachShot;
        switch (evenOrOdd)
        {
            case 0:
                distanceBetweenEachShot = _shotPosNextToPlayer / (_bulletCount -1);
                for (int i = 0; i < _bulletCount; i++)
                {
                    Vector3 shootPos = new Vector3(playerPos.x - _shotPosNextToPlayer / 2, bulletSpawnPos.y, playerPos.z);
                    shootPos.x += distanceBetweenEachShot * i;
                    GameObject createBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                    createBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    createBullet.SetActive(true);
                    createBullet.transform.position = bulletSpawnPos;
                    createBullet.GetComponent<Bullet>().bulletSpeed = _speed;
                    createBullet.GetComponent<Bullet>().ShootOnThisPos(shootPos, bulletSpawnPosGameObject.transform.position.y, bulletHealth);
                }
                break;
            case 1:
                distanceBetweenEachShot = _shotPosNextToPlayer / (_bulletCount -1);
                for (int i = 0; i < _bulletCount; i++)
                {
                    Vector3 shootPos = new Vector3(playerPos.x - _shotPosNextToPlayer / 2, bulletSpawnPos.y, playerPos.z);
                    shootPos.x += distanceBetweenEachShot * i;
                    GameObject createBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                    createBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    createBullet.SetActive(true);
                    createBullet.transform.position = bulletSpawnPos;
                    createBullet.GetComponent<Bullet>().bulletSpeed = _speed;
                    createBullet.GetComponent<Bullet>().ShootOnThisPos(shootPos, bulletSpawnPosGameObject.transform.position.y, bulletHealth);
                }
                break;
        }
    }

    #region Feint

    public void SpawnFeint(float _speed, float _focusPlayerAfterTime, int _bulletCount)
        {
            StartCoroutine(FeintShoot(_speed, _focusPlayerAfterTime, _bulletCount));
        }
    
        IEnumerator FeintShoot(float _speed, float _focusPlayerAfterTime, int _bulletCount)
        {
            //Finten Schuss
            Vector3 bulletSpawnPos = transform.position;
            bulletSpawnPos.y = bulletSpawnPosGameObject.transform.position.y;
            //Spawn Bestimmte Anzahl an Kugeln
            for (int i = 0; i < _bulletCount; i++)
            {
                int randomNum = Random.Range(-15, 16);
                Vector3 shootPos = new Vector3(playerPos.x + randomNum, bulletSpawnPos.y, playerPos.z + randomNum);
                GameObject createBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                createBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                createBullet.SetActive(true);
                createBullet.transform.position = bulletSpawnPos;
                createBullet.GetComponent<Bullet>().bulletSpeed = _speed;
                createBullet.GetComponent<Bullet>().ShootOnPlayer(bulletHealth, false, shootPos);
                StartCoroutine(ActivateFeint(_focusPlayerAfterTime, createBullet));
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
    
        IEnumerator ActivateFeint(float _focusPlayerAfterTime, GameObject _createdBullet)
        {
            //Momentanes Movement Abbrechen + dann auf Player zusteuern
            yield return new WaitForSecondsRealtime(_focusPlayerAfterTime);
            _createdBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _createdBullet.GetComponent<Bullet>().ShootOnPlayer(bulletHealth, false, new Vector3(playerPos.x + Random.Range(-3,3), bulletSpawnPosGameObject.transform.position.y,  playerPos.z + Random.Range(-3, 3)));
        }

    #endregion
    

    #endregion
    
    #region Special Type of Attack

    //Funktion um eine Wolke spawnen zu lassen
    public void SpawnCloud()
    {
        Vector3 cloudSpawnPos = playerPos;
        cloudSpawnPos.y += 5;
        cloudSpawnPos.x += Random.Range(-5, 5);
        cloudSpawnPos.z += Random.Range(-3, 3);
        spawnedCloud = Instantiate(cloud, cloudSpawnPos, Quaternion.identity);
        spawnedCloud.transform.parent = this.transform;
    }

    public void SpawnComet()
    {
        GameObject playerPosition = GameObject.FindGameObjectWithTag("Player");
        playerPosition.transform.position = new Vector3(playerPosition.transform.position.x,
            bulletSpawnPosGameObject.transform.position.y, playerPosition.transform.position.z);
        GameObject spawnComet = Instantiate(comet, bulletSpawnPosGameObject.transform.position, Quaternion.identity);
        spawnComet.transform.parent = this.transform;
        spawnComet.GetComponent<Comet>().ShootOnThisPos(bulletSpawnPosGameObject, cometShootPos, playerPosition);
    }

    public void SpawnTurret(float _bulletSpeed)
    {
        Vector3 playerPositon = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 shootPos = new Vector3(playerPositon.x + Random.Range(-2, 3),
            bulletSpawnPosGameObject.transform.position.y, playerPositon.z + Random.Range(-2, 3));
        
        GameObject spawnedTurret =
            Instantiate(turret, bulletSpawnPosGameObject.transform.position, Quaternion.identity);
        spawnedTurret.transform.parent = transform;
        spawnedTurret.GetComponent<Turret>().ShootOnThisPos(shootPos, bulletSpawnPosGameObject.transform.position.y, bulletHealth, _bulletSpeed);
    }
    #endregion
    
    #region Cone Attack

    public void SpawnBulletsInCone(float _speed, bool _leftOrRight)
    {
        //
        //Ändert die Form je nachdem wo der Spieler gerade steht
        //
        
        StartCoroutine(SpawnBulletsInConeCoroutine(_speed, _leftOrRight));
    }

    IEnumerator SpawnBulletsInConeCoroutine(float _speed, bool _leftOrRight)
    {
        //Left = false
        //Right = true
        Vector3 bulletSpawnPos = this.transform.position;
        bulletSpawnPos.y = bulletSpawnPosGameObject.transform.position.y;
        int randomBullets = Random.Range(9, 15);
        Vector3 startPos = leftCube.transform.position;
        Vector3 endPos = rightCube.transform.position;

        if (!_leftOrRight)
        {
            float distanceX = Mathf.Abs(startPos.x - endPos.x);
            float singlePos = distanceX / randomBullets;

            for (int i = 0; i <= randomBullets; i++)
            {
                startPos.x += singlePos * i;
                GameObject shootPos = Instantiate(leftCube, startPos, Quaternion.identity);
                spawnedBullet = Instantiate(bullet, bulletSpawnPos, Quaternion.identity);
                spawnedBullet.transform.parent = this.transform;
                spawnedBullet.GetComponent<Bullet>().bulletSpeed = _speed;
                spawnedBullet.GetComponent<Bullet>().ShootOnThisPos(startPos, bulletSpawnPosGameObject.transform.position.y, bulletHealth);
                startPos = leftCube.transform.position;
                yield return new WaitForSecondsRealtime(coneAttackSecondsBetweenEachShot);
                Destroy(shootPos);
            }
        }
        else
        {
            float distanceX = startPos.x - endPos.x;
            float distanceBetweenPos = distanceX / randomBullets;

            for (int i = 0; i <= randomBullets; i++)
            {
                endPos.x += distanceBetweenPos * i;
                GameObject shootPos = Instantiate(leftCube, endPos, Quaternion.identity);
                spawnedBullet = Instantiate(bullet, bulletSpawnPos, Quaternion.identity);
                spawnedBullet.transform.parent = this.transform;
                spawnedBullet.GetComponent<Bullet>().bulletSpeed = _speed;
                spawnedBullet.GetComponent<Bullet>().ShootOnThisPos(endPos, bulletSpawnPosGameObject.transform.position.y, bulletHealth);
                endPos = rightCube.transform.position;
                yield return new WaitForSecondsRealtime(coneAttackSecondsBetweenEachShot);
                Destroy(shootPos);
            }
        }
    }

    public void SpawnBulletsInVFormation(float _speed, float _distanceBetween, float _seconds, int _bulletSpawns)
    {
        GameObject spawnVFormationGo = Instantiate(VFormationGo, bulletSpawnPosGameObject.transform.position, Quaternion.identity);
        spawnVFormationGo.transform.LookAt(new Vector3(playerPos.x, bulletSpawnPosGameObject.transform.position.y, playerPos.z));
        spawnVFormationGo.GetComponent<VFormationAttack>().SpawnVFormation(_bulletSpawns, _distanceBetween, _seconds, _speed);
    }
    //DistanceBetween = Distanz zwischen jeder Kugel
    //Seconds = Sekunden, wann die nächste Reihe von Kugeln spawnen soll / Bsp.: Distanz zwischen Kugel 1 und 2/3
    //Intervalls = Falls mehrere Kugel Reihen spawnen sollen
    //BulletSpawns = Wieviele Kugeln insgesamt Spawnen sollen
    //ShootPos = Für AroundMe, damit man auch auf andere Pos schießen kann.

    #endregion

    #region Around Me Attack

    // Attack muss gefixed werden
    
    public void SpawnBulletsAroundMe(int _shootingWaves, float _secondsBetweenWave, int _numOfBullets, float _speed)
    {
        StartCoroutine(ShootWaves(_shootingWaves, _secondsBetweenWave, _numOfBullets, _speed));
    }

    IEnumerator ShootWaves(int _shootingWaves, float _secondsBetweenWave,int _numOfBullets, float _speed )
    {
        Debug.Log("Ciaoo");
        float radius = 5;
        for(int j = 0; j < _shootingWaves; j++)
        {
            _numOfBullets += Random.Range(-3, 3);
            for (int i = 0; i < (float)_numOfBullets; i++)
            {
                float radians = 2 * Mathf.PI / (float)_numOfBullets * i;

                var vertical = Mathf.Sin(radians);
                var horizontal = Mathf.Cos(radians);

                var spawnDir = new Vector3(horizontal, bulletSpawnPosGameObject.transform.position.y, vertical);

                var spawnPos = this.transform.position + spawnDir * radius;
                var shootingPoint = Instantiate(bulletSpawnPosGameObject, spawnPos, Quaternion.identity);
                Destroy(shootingPoint, 0.1f);
                shootingPoint.transform.parent = this.transform;
                GameObject spawnBullet = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                spawnBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                spawnBullet.SetActive(true);
                spawnBullet.transform.position = bulletSpawnPosGameObject.transform.position;
                spawnBullet.GetComponent<Bullet>().bulletSpeed = _speed;
                spawnBullet.GetComponent<Bullet>().ShootOnThisPos(spawnPos,
                    transform.GetComponent<CreateAttack>().bulletSpawnPosGameObject.transform.position.y,
                    transform.GetComponent<CreateAttack>().bulletHealth);
            }
            yield return new WaitForSeconds(_secondsBetweenWave);
        }
    }

    public void SpawnBulletsInSpirale(int _bulletCount, float _speed, float _duration)
    {
        StartCoroutine(SpinBulletsSpawnPos(_bulletCount, _speed, _duration));
    }

    IEnumerator SpinBulletsSpawnPos(int _bullets, float _speed , float _duration)
    {
        float secondsBetweenEachShot = _duration / _bullets;
        bulletSpawnPosGameObject.transform.rotation= Quaternion.Euler(0, Random.Range(0, 361), 0);
        
        Quaternion startRotation = bulletSpawnPosGameObject.transform.rotation;
        float t = 0.0f;
        while (t <= _duration)
        {
            t += secondsBetweenEachShot;
            bulletSpawnPosGameObject.transform.rotation =
                startRotation * Quaternion.AngleAxis(t / _duration * 360f, Vector3.up);
            //Um Vor sich zu schießen
            GameObject createBulletFront = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
            createBulletFront.GetComponent<Rigidbody>().velocity = Vector3.zero;
            createBulletFront.SetActive(true);
            createBulletFront.transform.position = bulletSpawnPosGameObject.transform.position;
            Vector3 shootOnThisPosFront =
                bulletSpawnPosGameObject.transform.position + bulletSpawnPosGameObject.transform.forward;
            createBulletFront.GetComponent<Bullet>().bulletSpeed = _speed;
            createBulletFront.GetComponent<Bullet>().ShootOnThisPos(shootOnThisPosFront, bulletSpawnPosGameObject.transform.position.y, 2);
            //Um Hinter sich zu schießen
            GameObject createBulletBack = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
            createBulletBack.GetComponent<Rigidbody>().velocity = Vector3.zero;
            createBulletBack.SetActive(true);
            createBulletBack.transform.position = bulletSpawnPosGameObject.transform.position;
            Vector3 shootOnThisPosBack =
                bulletSpawnPosGameObject.transform.position - bulletSpawnPosGameObject.transform.forward;
            createBulletBack.GetComponent<Bullet>().bulletSpeed = _speed;
            createBulletBack.GetComponent<Bullet>().ShootOnThisPos(shootOnThisPosBack, bulletSpawnPosGameObject.transform.position.y, 2);
            yield return new WaitForSecondsRealtime(secondsBetweenEachShot);
            yield return null;
        }
        bulletSpawnPosGameObject.transform.rotation = startRotation;
    }

    public void SpawnVFormationAroundMe(float _speed, float _distanceBetween, float _seconds, int _numberOfVFormation, int _bulletSpawns)
    {
        //
        // Muss gefixed werden Kugeln spawnen nicht richtig
        //
        float radius = 5;
        for (int i = 0; i < _numberOfVFormation; i++)
        {
            Vector3 bulletSpawnPos = new Vector3(transform.position.x, bulletSpawnPosGameObject.transform.position.y,
                transform.position.z);
            float radians = 2 * Mathf.PI / _numberOfVFormation * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, bulletSpawnPosGameObject.transform.position.y, vertical);
            var spawnPos = new Vector3(transform.position.x + spawnDir.x * radius, bulletSpawnPosGameObject.transform.position.y, transform.position.z + spawnDir.z * radius);
            GameObject spawnVFormationsGo;
            spawnVFormationsGo = Instantiate(VFormationGo, bulletSpawnPos, Quaternion.identity, transform);
            spawnVFormationsGo.transform.LookAt(spawnPos);
            spawnVFormationsGo.GetComponent<VFormationAttack>().SpawnVFormation(_bulletSpawns, _distanceBetween, _seconds, _speed);
            Destroy(spawnVFormationsGo, 10);
        }
        
    }
    
    #endregion

    #region Laser

    public void SpawnLaser()
    {
        StartCoroutine(SpawnLaserCoroutine());

    }

    IEnumerator SpawnLaserCoroutine()
    {
        leftLaser.GetComponent<ParticleSystem>().Play();
        rightLaser.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(1f);
        leftLaser.GetComponent<Laser>().checkIfLaserStarted = true;
        rightLaser.GetComponent<Laser>().checkIfLaserStarted = true;
    }
    public void EndLaser()
    {
        
    }

    #endregion

    #region Explosion

    public void SpawnSplittingExplodingBullet(float _speed, int _howManyBulletsShouldSpawn, float _normalBulletSpeed)
    {
        explotionCooldown = true;
        Vector3 bulletSpawnPos = new Vector3(transform.position.x, bulletSpawnPosGameObject.transform.position.y, transform.position.z);
        Vector3 shootPos = new Vector3(playerPos.x + Random.Range(-5, 6), bulletSpawnPosGameObject.transform.position.y, playerPos.z + Random.Range(-5,6));
        GameObject spawnExplosivBullet = BulletsToLoad.instance.explosivBulletsQueue.Dequeue();
        spawnExplosivBullet.SetActive(true);
        spawnExplosivBullet.transform.position = bulletSpawnPos;
        spawnExplosivBullet.GetComponent<ExplodingBullet>().bulletSpeed = _speed;
        spawnExplosivBullet.GetComponent<ExplodingBullet>().ExplodingPos(_howManyBulletsShouldSpawn, _normalBulletSpeed, shootPos);
        spawnExplosivBullet.GetComponent<ExplodingBullet>().explotionTimer = Random.Range(1.0f, 2.0f);
    }

    #endregion

    #region Splitting Bullet

    public void SpawnSplittingBullet(float _speed, float _timeBetweenEachShot, int _bulletHealth, float _splittedBulletSpeed, int _splittedBulletHealth)
    {
        Vector3 bulletSpawnPos = new Vector3(transform.position.x, bulletSpawnPosGameObject.transform.position.y,
            transform.position.z);
        Vector3 shootPos = new Vector3(playerPos.x + Random.Range(-3, 4), bulletSpawnPosGameObject.transform.position.y,
            playerPos.z + Random.Range(-3, 4));
        GameObject createSplitBullet = Instantiate(splittingBullet, bulletSpawnPos, Quaternion.identity, transform);
        createSplitBullet.transform.LookAt(shootPos);
        createSplitBullet.GetComponent<SplittingBullet>().bulletSpeed = _speed;
        createSplitBullet.GetComponent<SplittingBullet>().ShootOnThisPos(shootPos, _timeBetweenEachShot, _bulletHealth, _splittedBulletSpeed, _splittedBulletHealth);
    }

    #endregion
}
