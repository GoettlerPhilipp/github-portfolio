using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySmall : MonoBehaviour
{
    public bool inTotem;
    [Tooltip("Die Totems zu welchen dieser Charakter springen kann. Gameobjekte zu welchen er springen darf, müssen hier hineingezogen werden.")]
    public List<GameObject> Totems;
    private GameObject currentTotem;
    [HideInInspector]public GameObject nextTotem;
    
    [Tooltip("Gelber Gizmos ist die Jump Distanz")]
    public float jumpDistance = 10;
    [Tooltip("Roter Gizmos ist die Distanz zum Spieler")]
    public float distanceToPlayer = 20;
    
    
    public int health;
    [SerializeField]private float movementSpeed = 1f;
    [HideInInspector]public float interp;
    
    
    private EnemySmallState currentState;

    #region Attack Components
    
    [Tooltip("Sollte nicht höher als Angriffe in EnemySmallState sein, sonst greift der Gegner seltener an")]
    public int maxAttacks;
    [Tooltip("Wieviele Sekunden vergehen sollen, nachdem ein Angriff fertig ist, bis der State gewechselt werden soll.")]
    public float switchStateCooldown;

    [Header("Normal Bullet")] 
    public float normalBulletSpeed;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Zuständig wieviele Kugeln spawnen sollen")]
    public Vector2 normalBulletExecute;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Für Timer zuständig zwischen jedem Schuss")]
    public Vector2 normalBulletExecutionCooldown;
    
    [Header("Follow Player Bullet")] 
    public float followPlayerBulletSpeed;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Zuständig wieviele Kugeln spawnen sollen")]
    public Vector2 followPlayerBulletCount;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Für Timer zuständig zwischen jedem Schuss")]
    public Vector2 followPlayerBulletExecutionCooldown;
    
    [Header("Exploding Bullet Attack")] 
    public float explodingBulletBulletSpeed;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Wieviele Kugeln nach der Explosion spawnen")]
    public Vector2 explodingBulletBulletsToSpawn;
    
    [Header("Feint Bullet")]
    public float feintBulletSpeed;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Zuständig wieviele Kugeln spawnen sollen")]
    public Vector2 feintBulletCount;
    public float feintFocusAfterTime;
    
    # endregion
    
    private void Awake()
    {
        float maxDistance = 50;
        for (int i = 0; i < Totems.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, Totems[i].transform.position);
            if (distance < maxDistance && !Totems[i].GetComponent<Totems>().enemyOnMe)
            {
                maxDistance = distance;
                currentTotem = Totems[i];
            }
        }
        currentTotem.GetComponent<Totems>().enemyOnMe = true;
        transform.position = currentTotem.transform.position;
        inTotem = true;
        foreach (Transform child in gameObject.transform)
            child.GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = new IDLE_Small(gameObject);
    }

    

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
        /*if (Input.GetKeyUp(KeyCode.N))
        {
            List<GameObject> jumpableTotems = new List<GameObject>();
            for (int i = 0; i < Totems.Count; i++)
            {
                Debug.Log("Distanz von "+ i + " : " + Vector3.Distance(currentTotem.transform.position, Totems[i].transform.position));
                if (!Totems[i].GetComponent<Totems>().enemyOnMe)
                {
                    float distance = Vector3.Distance(currentTotem.transform.position, Totems[i].transform.position);
                    if(distance < jumpDistance)
                        jumpableTotems.Add(Totems[i]);
                }
            }

            int ran = Random.Range(0, jumpableTotems.Count);
            nextTotem = jumpableTotems[ran];
        }
        */
        if(nextTotem != null)
            JumpToTotem();
    }

    public void JumpToTotem()
    {
        if (inTotem)
        {
            inTotem = false;
            foreach (Transform child in gameObject.transform)
                child.GetComponent<MeshRenderer>().enabled = true;
            transform.LookAt(nextTotem.transform);
            currentTotem.GetComponent<Totems>().enemyOnMe = false;
            currentTotem.transform.GetChild(0).gameObject.SetActive(false);
            nextTotem.GetComponent<Totems>().enemyOnMe = true;
        }
        
        if (interp < 1)
            interp += Time.deltaTime * movementSpeed;
        transform.position = Vector3.Lerp(currentTotem.transform.position, nextTotem.transform.position, interp);

        if (interp >= 1)
        {
            inTotem = true;
            foreach (Transform child in gameObject.transform)
                child.GetComponent<MeshRenderer>().enabled = false;
            currentTotem = nextTotem;
            currentTotem.transform.GetChild(0).gameObject.SetActive(true);
            nextTotem = null;
            interp = 0;
        }
    }

    public void DestroyMe()
    {
        currentTotem.GetComponent<Totems>().enemyOnMe = false;
        Destroy(this);
        Destroy(this.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToPlayer);
    }
}
