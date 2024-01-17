using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class EnemyBoss_1 : MonoBehaviour
{
    //Skript für den Boss, damit er in die FSM geht.
    public EnemyBossState_1 currentState;

    [HideInInspector]public EnemyBossController bossController;

    [HideInInspector]public int bossHealth;
    

    
    private GameObject player;
    [SerializeField] private float interactRange = 6f;
    

    public string name;

    #region Attack Components
    
    [Tooltip("Wieviele Sekunden vergehen sollen, nachdem ein Angriff fertig ist, bis der State gewechselt werden soll.")]
    public float switchStateCooldown;
    #region Basic Attacks
    
    [Header("Normal Bullet")] 
        public float normalBulletSpeed;
        [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Zuständig wieviele Kugeln spawnen sollen")]
        public Vector2 normalBulletExecute;
        [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Für Timer zuständig zwischen jedem Schuss")]
        public Vector2 normalBulletExecutionCooldown;
        [Range(1.0f, 2.0f)] public float normalBulletMultiplier = 1.5f;
    
    [Header("Follow Player Bullet")] 
        public float followPlayerBulletSpeed;
        [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Zuständig wieviele Kugeln spawnen sollen")]
        public Vector2 followPlayerBulletCount;
        [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Für Timer zuständig zwischen jedem Schuss")]
        public Vector2 followPlayerBulletExecutionCooldown;
        [Range(1.0f, 2.0f)] public float followPlayerBulletMultiplier = 1.5f;
        
    [Header("Feint Bullet")]
        public float feintBulletSpeed;
        [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Zuständig wieviele Kugeln spawnen sollen")]
        public Vector2 feintBulletCount;
        public float feintFocusAfterTime;
        [Range(1.0f, 2.0f)] public float feintMultiplier = 1.5f;

    [Header("Cone Attack")] 
        public float coneBulletSpeed;
        [Range(1.0f, 2.0f)] public float coneBulletMultiplier = 1.5f;

    [Header("Multi Attack")] 
        public float multiAttackBulletSpeed;
        [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Wieviele Kugeln pro Reihe spawnen")]
        public Vector2 multiAttackBulletCount;
        public int multiAttackExecution;
        public float multiAttackDistanceBetweenEachBullet;
        [Range(1.0f, 2.0f)] public float multiAttackMultiplier = 1.5f;
        public float multiAttackSecondsBetweenEachWave;

    [Header("V Formation")] 
        public float vFormationBulletSpeed;
        public Vector2 vFormationBulletCount;
        public int vFormationExecution;
        public float vFormationDistanceBetweenEachBullet;
        [Range(1.0f, 2.0f)] public float vFormationMultiplier = 1.5f;
        public float vFormationSeconds;
        
    #endregion

    #region Around Me Attacks

    [Header("Basic Around Me")] 
        public float aroundMeBulletSpeed;
        public Vector2 aroundMeBulletCount;
        public int aroundMeWaveCounter;
        public float aroundMeSecondsBetweenEachWave;
        [Range(1.0f, 2.0f)] public float aroundMeMultiplier = 1.5f;
        
    [Header("Spirale")] 
        public float spiraleBulletSpeed;
        public Vector2 spiraleBulletCount;
        public float spiraleDuration;
        [Range(1.0f, 2.0f)] public float spiraleMultiplier = 1.5f;
        
    [Header("V Formation Around Me")] 
        public float vFormationAmBulletSpeed;
        public Vector2 vFormationAmBulletCount;
        public float vFormationAmDistanceBetweenEachBullet;
        [Range(1.0f, 2.0f)] public float vFormationAmMultiplier = 1.5f;
        public float vFormationAmSeconds;
        public Vector2 vFormationAmFormationCount;

    #endregion
    
    [Header("Exploding Bullet Attack")] 
    public float explodingBulletBulletSpeed;
    [Tooltip("Vector X ist niedrigstes, Vector Y is maximale Bullets. Wieviele Kugeln nach der Explosion spawnen")]
    public Vector2 explodingBulletBulletsToSpawn;
    
    
    # endregion
    

    // Start is called before the first frame update
    void Start()
    {
        bossController = gameObject.GetComponent<EnemyBossController>();
        bossHealth = bossController.bossHealth;
        player = GameObject.FindWithTag("Player");
        currentState = new IDLE_Boss_1(this.gameObject, this);
    }

    // Update is called once per frame
    void Update()
    {
        if(bossHealth <= 0)
            return;
        bossHealth = bossController.bossHealth;
        currentState = currentState.Process();
        name = currentState.name.ToString();
        
        if (!bossController.startBoss)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            if (distance <= interactRange)
            {
                //BattleScenePlayerMovement -> PlayerController
                //player.GetComponent<BattleScenePlayerMovement>().enemyBoss = gameObject;
                //player.GetComponent<BattleScenePlayerMovement>().canInteractWithBoss = true;

                PlayerController.instance.enemyBoss = this.gameObject;
                PlayerController.instance.canInteractWithBoss = true;
            }
            else
            {
                
                //player.GetComponent<BattleScenePlayerMovement>().enemyBoss = null;
                //player.GetComponent<BattleScenePlayerMovement>().canInteractWithBoss = false;
                PlayerController.instance.enemyBoss = null;
                PlayerController.instance.canInteractWithBoss = false;
            }
        }
    }
    
    private void OnTriggerEnter(Collider _collider)
    {
        if(_collider.CompareTag("Player"))
            Debug.Log("Player In Me");
    }

    private void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Player"))
        {
            bossController.startBoss = false;
            currentState = new IDLE_Boss_1(this.gameObject, this);
            Debug.Log("Player left");
        }
    }
}
