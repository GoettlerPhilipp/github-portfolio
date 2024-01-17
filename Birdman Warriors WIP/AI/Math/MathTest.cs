using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTest : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public GameObject midPos;

    private float maxDis = 30f;  //A Halbiert

    [Range(0, 30)][SerializeField] private float distanceAtm;
    [Range (0, 5)] [SerializeField] private float height;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        /*float distance = rightCube.transform.position.x - leftCube.transform.position.x;
        distance = distance / 100; 
        sliderCubePos.x = leftCube.transform.position.x + distance * sliderPos;
        sliderCube.transform.position = new Vector3(sliderCubePos.x, sliderCubePos.y, sliderCubePos.z);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(new Vector3(player.position.x, 0, player.position.z),
                new Vector3(enemy.position.x, 0, enemy.position.z)) <= 30 && Vector3.Distance(
                new Vector3(player.position.x, 0, player.position.z),
                new Vector3(enemy.position.x, 0, enemy.position.z)) >= 5)
        {
            float distanceFromPlayerToBoss = maxDis - Vector3.Distance(player.position, enemy.position);
            distanceAtm = distanceFromPlayerToBoss;
            height = distanceAtm / 5;

            Vector3 midPosMath = Vector3.Lerp(player.position, enemy.position, 0.5f);
            midPos.transform.position = new Vector3(midPosMath.x, midPosMath.y + 5 + height, midPosMath.z);
        }
    }
}
