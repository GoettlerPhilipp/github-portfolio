using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    private Rigidbody rig;
    public float bulletSpeed;
    private static Vector3 startPos;
    private Vector3 direction;
    public List<Vector3> currentList;
    [SerializeField] private GameObject burningGround;
    

    private void Awake()
    {
        startPos = transform.position;
        rig = this.GetComponent<Rigidbody>();
        StartCoroutine(ActivateCollider());
        Destroy(gameObject, 4);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(gameObject.transform.position, currentList[2]) < 0.7f)
        {
            CreateBurningGround();
            Destroy(gameObject);
        }
    }

    public void ShootOnThisPos(GameObject _startPos, GameObject _midPos, GameObject _endPos)
    {
        currentList.Add(_startPos.transform.position);
        currentList.Add(_midPos.transform.position);
        currentList.Add(_endPos.transform.position);
    }

    void CreateBurningGround()
    {
        GameObject burningGroundGo = Instantiate(burningGround, transform.position, Quaternion.identity);
        burningGroundGo.transform.parent = transform.parent;
    }
    

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GetComponent<Collider>().isTrigger = false;
    }
}
