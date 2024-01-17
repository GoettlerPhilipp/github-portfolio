using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsToLoad : MonoBehaviour
{
    public static BulletsToLoad instance;
    [SerializeField] private GameObject normalBulletsParent;
    public Queue<GameObject> normalBulletsQueue = new Queue<GameObject>();
    [SerializeField] private GameObject explosivBulletsParent;
    public Queue<GameObject> explosivBulletsQueue = new Queue<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        foreach (Transform child in normalBulletsParent.transform)
            normalBulletsQueue.Enqueue(child.gameObject);
        foreach (Transform child in explosivBulletsParent.transform)
            explosivBulletsQueue.Enqueue(child.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Queue: " + normalBulletsQueue.Count);
    }
}
