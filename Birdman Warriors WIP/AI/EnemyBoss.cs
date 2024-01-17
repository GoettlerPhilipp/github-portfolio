using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBoss : MonoBehaviour
{
    //Skript für den Boss, damit er in die FSM geht.
    public EnemyBoss_State currentState;

    public int bossHealth;

    
    private GameObject player;
    [SerializeField] private float interactRange = 6f;
    [HideInInspector] public bool startBoss;
    
    
    [HideInInspector]public bool getKnockback;
    [SerializeField] private GameObject knockbackPos;
    public int interpolationFramesCount = 45;
    private int elapsedFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        currentState = new IDLE_Boss(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
        
        if(getKnockback)
            KnockBackPlayer();
        
        if (!startBoss)
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

    public void StartBossFight()
    {
        player.GetComponent<PlayerController>().enemyBoss = null;
        player.GetComponent<PlayerController>().canInteractWithBoss = false;
        player.GetComponent<CharacterController>().enabled = false;
        startBoss = true;
    }

    public void KnockBackPlayer()
    {
        //Weitere Möglichkeit wäre es, den Player in einer Bogenbewegung zu bewegen.
        //https://youtu.be/ddakS7BgHRI?si=SX2pjq6dcWtblDX1
        //Video, genau für die Bogen Bewegung.
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        player.GetComponent<CharacterController>().enabled = false;
        
        player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(knockbackPos.transform.position.x, knockbackPos.transform.position.y + 2, knockbackPos.transform.position.z), interpolationRatio / 15);
        
        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);

        //Debug.Log("Elapsed: " + elapsedFrames);
        if (elapsedFrames <= 0)
        {
            player.GetComponent<CharacterController>().enabled = true;
                    getKnockback = false;
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
            startBoss = false;
            
            currentState = new IDLE_Boss(this.gameObject);
            Debug.Log("Player left");
        }
    }
}
