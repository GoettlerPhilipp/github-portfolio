using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;

    private CreateAttack enemyParent;

    private GameObject leftCube;
    private GameObject rightCube;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyParent = transform.parent.gameObject.GetComponent<CreateAttack>();
        leftCube = enemyParent.leftCube;
        rightCube = enemyParent.rightCube;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = player.position - transform.position;
        //transform.LookAt(-player.position);
        Quaternion rotation = Quaternion.LookRotation(-relativePos);
        transform.rotation = rotation;
        leftCube.transform.position = new Vector3(leftCube.transform.position.x, enemyParent.bulletSpawnPosGameObject.transform.position.y, leftCube.transform.position.z);
        rightCube.transform.position = new Vector3(rightCube.transform.position.x, enemyParent.bulletSpawnPosGameObject.transform.position.y, rightCube.transform.position.z);
    }
}
