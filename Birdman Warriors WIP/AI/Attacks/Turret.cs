using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Rigidbody turretRig;
    
    public int turretHealth;
    
    private Vector3 direction;
    private Vector3 shootPos;
    private float rotSpeed;
    public float turretSpeed;
    private bool stationary;
    private float cooldown;

    private CreateAttack createAttacks;
    [SerializeField] private float bulletSpeed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        turretRig = GetComponent<Rigidbody>();
        createAttacks = transform.parent.GetComponent<CreateAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turretHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (stationary)
        {
            if (cooldown <= 0)
            {
                StartCoroutine(SpawnBulletsOverTime());
                cooldown = 3f;
            }
            else
            {
                cooldown -= Time.deltaTime;
            }
        }
        else
        {
            direction = direction.normalized;
            if (Vector3.Distance(transform.position, shootPos) > 2)
            {
                turretRig.AddForce(direction * (turretSpeed * Time.deltaTime) * 4, ForceMode.Force);
            }
            else
            {
                stationary = true;
                turretRig.velocity = Vector3.zero;
            }
        }
    }
    
    public void ShootOnThisPos(Vector3 _shootPos, float _height, int _bulletHealth, float _bulletSpeed)
    {
        turretHealth = _bulletHealth;
        _shootPos.y = _height;
        shootPos = _shootPos;
        bulletSpeed = _bulletSpeed;
        direction = _shootPos - this.transform.position;
    }
    
    public void SpawnBullet(float _speed)
    {
        Vector3 bulletSpawnPos = this.transform.position;
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 shootPos = new Vector3(playerPos.x + Random.Range(-5 , 5), bulletSpawnPos.y, playerPos.z + Random.Range(-5, 5));
        GameObject spawnedBullet = Instantiate(createAttacks.bullet, bulletSpawnPos, Quaternion.identity);
        spawnedBullet.GetComponent<Bullet>().bulletSpeed = _speed;
        spawnedBullet.transform.parent = this.transform;
        spawnedBullet.GetComponent<Bullet>().ShootOnPlayer(createAttacks.bulletHealth, false, shootPos);
    }

    IEnumerator SpawnBulletsOverTime()
    {
        int randomNum = Random.Range(4, 9);
        for (int i = 0; i < randomNum; i++)
        {
            SpawnBullet(bulletSpeed);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
