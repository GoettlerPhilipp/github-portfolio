using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerUpperworldDamage : MonoBehaviour
{
    [Header("Properties")] 
    public int damageAgainstWall;
    public float attackRange;
    public float attackSpeed;
    [SerializeField] private bool isAttacking;

    [Header("Components")] 
    private PauseMenuController pauseMenuController;
    private PlayerMovement playerMovement;
    private GameObject targetWall;
    private Camera camera;

    [Header("Enemy Capture")] 
    public bool isCapturingEnemy;
    [SerializeField] private float captureRange;
    private EnemyBaseHealth enemyBaseHealth;

    [Header("Card Property")] 
    public bool wallProtected;
    
    private void Awake()
    {
        pauseMenuController = FindObjectOfType<PauseMenuController>();
        playerMovement = GetComponent<PlayerMovement>();
        GameObject.FindObjectOfType<IngameMenuController>().playerUpperworldDamage = this;
    }

    private void Update()
    {
        SelectWall();
        CaptureEnemyBase();
        
        camera = Camera.main;
        
        if (enemyBaseHealth != null)
        {
            if(isCapturingEnemy)
                enemyBaseHealth.StartCountdown();
            else
                enemyBaseHealth.ResetCountdown();

            if (enemyBaseHealth.currentTimerUntilCaptured < 0)
            {
                pauseMenuController.panelWinMenu.SetActive(true);
                enemyBaseHealth.ResetCountdown();
                isCapturingEnemy = false;
            }
        }
    }

    private void CaptureEnemyBase()
    {
        if (camera != null)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            if(Input.GetMouseButtonDown(1))
                if (Physics.Raycast(ray, out hit)) 
                {
                    if (hit.transform.CompareTag("EnemyMainBase") && IsInAttackange(hit.transform.position, captureRange))
                    {
                        enemyBaseHealth = hit.transform.gameObject.GetComponent<EnemyBaseHealth>();
                        isCapturingEnemy = true;
                        Debug.Log("Is Capturing");
                    }
                    else if(playerMovement.chooseUnit != null)
                        isCapturingEnemy = false;
                }
        }
    }

    private void SelectWall()
    {
        if (camera != null)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(1))
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform.gameObject.layer == 17 && IsInAttackange(hit.transform.position, attackRange))
                    {
                        if(targetWall != null)
                            targetWall.transform.GetComponent<WallHealthController>().isBeingAttacked = false;
                    
                        targetWall = hit.transform.gameObject;
                        targetWall.transform.GetComponent<WallHealthController>().isBeingAttacked = true;
                        playerMovement.DeselectPlayer();
                    }

            if(targetWall != null && !wallProtected)
                StartCoroutine(DamageWall(targetWall));
        }
    }

    private IEnumerator DamageWall(GameObject _wall)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _wall.transform.GetComponent<WallHealthController>().ReceiveDamage(damageAgainstWall);
            
            yield return new WaitForSeconds(attackSpeed);
            isAttacking = false;
        }
    }
    
    private bool IsInAttackange(Vector3 _wall, float _range)
    {
        var distance = Vector3.Distance(this.transform.position, _wall);
        return distance <= _range;
    }
}
