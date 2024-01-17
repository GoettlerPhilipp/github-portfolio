using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFormationAttack : MonoBehaviour
{
    public GameObject bullet;

    public void SpawnVFormation(int _bulletCount, float _distanceBetween, float _seconds, float _bulletSpeed)
    {
        StartCoroutine(SpawnVFormationCoroutine(_bulletCount, _distanceBetween, _seconds, _bulletSpeed));
    }

    IEnumerator SpawnVFormationCoroutine(int _bulletCount, float _distance, float _seconds, float _bulletSpeed)
    {
        for (int i = 0; i <= _bulletCount; i++)
        {
            if (i == 0)
            {
                GameObject spawnMiddle = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                spawnMiddle.GetComponent<Rigidbody>().velocity = Vector3.zero;
                spawnMiddle.SetActive(true);
                spawnMiddle.transform.position = transform.position;
                spawnMiddle.transform.rotation = transform.rotation;
                spawnMiddle.GetComponent<Rigidbody>().isKinematic = false;
                spawnMiddle.GetComponent<Bullet>().bulletSpeed = _bulletSpeed;
                spawnMiddle.GetComponent<Bullet>().vFormation = true;
            }
            else
            {
                GameObject spawnLeft = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                spawnLeft.GetComponent<Rigidbody>().velocity = Vector3.zero;
                spawnLeft.SetActive(true);
                GameObject spawnRight = BulletsToLoad.instance.normalBulletsQueue.Dequeue();
                spawnRight.GetComponent<Rigidbody>().velocity = Vector3.zero;
                spawnRight.SetActive(true);
                spawnLeft.transform.rotation = transform.rotation;
                spawnRight.transform.rotation = transform.rotation;
                spawnLeft.transform.position = new Vector3(transform.position.x + i * _distance, transform.position.y, transform.position.z + -i * _distance);
                spawnRight.transform.position = new Vector3(transform.position.x + -i * _distance, transform.position.y, transform.position.z + -i * _distance);
                spawnLeft.GetComponent<Rigidbody>().isKinematic = false;
                spawnRight.GetComponent<Rigidbody>().isKinematic = false;
                spawnLeft.GetComponent<Bullet>().bulletSpeed = _bulletSpeed;
                spawnRight.GetComponent<Bullet>().bulletSpeed = _bulletSpeed;
                spawnLeft.GetComponent<Bullet>().vFormation = true;
                spawnRight.GetComponent<Bullet>().vFormation = true;
            }
            yield return new WaitForSecondsRealtime(_seconds);
        }
    }
}
