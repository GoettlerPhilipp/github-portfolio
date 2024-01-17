using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    //Skript für das Bullet speed
    private Vector3 playerPos;
    private GameObject playerGo;
    private Rigidbody bulletRig;

    [HideInInspector] public Vector3 direction;
    private Vector3 dirFriendBul;

    public float bulletSpeed;
    public int bulletDamage;
    public int bulletHealth;

    //private float shootAngle = 30f;
    public bool vFormation;

    [SerializeField] public bool followPlayer;
    [SerializeField] private float rotSpeed;

    [HideInInspector] public float setGameObjectOff = 5;

    void Start()
    {
        if(gameObject.GetComponent<Rigidbody>())
            bulletRig = this.GetComponent<Rigidbody>();
        playerGo = GameObject.FindGameObjectWithTag("Player");
        if(followPlayer)
            transform.LookAt(playerPos);
        
    }


    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.black);
        if (bulletHealth <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (setGameObjectOff > 0)
            setGameObjectOff -= Time.deltaTime;
        else 
            gameObject.SetActive(false);
        
        if (!followPlayer)
        {
            
            direction = direction.normalized;

            if (gameObject.CompareTag("Bullet"))
            {
                if (!vFormation)
                {
                    //transform.position += transform.forward * bulletSpeed * Time.deltaTime / 10;
                    //bulletRig.AddRelativeForce(direction * (bulletSpeed * Time.deltaTime) * 10, ForceMode.Force);
                    bulletRig.AddForce(direction * (bulletSpeed * Time.deltaTime) * 10, ForceMode.Force);
                }
                else
                {
                    bulletRig.AddForce(transform.forward * (bulletSpeed * Time.deltaTime) * 10, ForceMode.Force);
                }
                
            }
            else if (gameObject.CompareTag("FriendlyBullet"))
                bulletRig.velocity = dirFriendBul *
                                     (bulletSpeed * PlayerController.instance.artefactEffects.paoaArtefacts.bulletSpeedPD6 *
                                      Time.deltaTime);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
            Quaternion lookAtPlayer = Quaternion.LookRotation(playerGo.transform.position - gameObject.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAtPlayer, rotSpeed * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime;
            gameObject.transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
            transform.position =
                new Vector3(transform.position.x, playerPos.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            if (!PlayerController.instance.artefactEffects.paoaArtefacts.isAttackingPD6 && !gameObject.CompareTag("FriendlyBullet"))
            {
                PlayerController.instance.playerHealth.DamageToPlayer(bulletDamage);
                gameObject.SetActive(false);
            }
            else
            {
                tag = "FriendlyBullet";
                dirFriendBul = PlayerController.instance.playerModel.forward;
                
            }
        }
        else if (_collider.CompareTag("Cover"))
        {
            _collider.GetComponent<CoverHealth>().GetDamage();
            gameObject.SetActive(false);
        }
        else if (_collider.CompareTag("FriendlyBullet"))
        {
            Destroy(_collider.gameObject);     //Instead DamageCalculation of the opposite Bullet?
            Destroy(gameObject);
        }
    }

    public void ShootOnPlayer(int _bulletHealth, bool _followPlayer, Vector3 _pos)
    {
        if (followPlayer)
            setGameObjectOff = 10;
        else
            setGameObjectOff = 5;
        BulletsToLoad.instance.normalBulletsQueue.Enqueue(gameObject);
        transform.LookAt(_pos);
        bulletHealth = _bulletHealth;
        followPlayer = _followPlayer;
        
        playerPos = _pos;
        
        direction = playerPos - this.transform.position;
    }

    public void ShootOnThisPos(Vector3 _shootPos, float _height, int _bulletHealth)
    {
        setGameObjectOff = 5;
        BulletsToLoad.instance.normalBulletsQueue.Enqueue(gameObject);
        bulletHealth = _bulletHealth;
        _shootPos.y = _height;
        direction = _shootPos - this.transform.position;
        followPlayer = false;
    }

    
}
