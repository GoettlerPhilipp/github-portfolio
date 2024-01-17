using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using EasyButtons;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Player movement on the overworld. 
    
    [Header("Movement")]
    [HideInInspector] public NavMeshAgent player;
    public static bool playerIsMoving;  // FOG of War noch ändern
    private bool mouseOverPlayer;

    [Header("Selection")]
    public GameObject chooseUnit;
    public NavMeshAgent chooseAgent;
    public GameObject playerUI;
    private Vector3 lastPos;
    
    [Header("Misc.")]
    public LineRenderer drawLine;

    private void Awake()
    {
        player = GetComponent<NavMeshAgent>();
        drawLine = GetComponent<LineRenderer>();

        player.updateRotation = false;

        drawLine.startWidth = 0.15f;
        drawLine.endWidth = 0.15f;
        drawLine.positionCount = 0;
    }

    private void Update()
    {
        if (PauseMenuController.instance.currentGameState == GameState.Paused)
            return;

        if(!TriggerBattle.startedBattle)
            ChosenUnit();
        
        if(player.hasPath)
            DrawPath();

        if (player.remainingDistance <= 1f || DialogueManager.isInDialogue)
            player.isStopped = true;
        else
            player.isStopped = false;
    }

    private void FixedUpdate()
    {
        FaceMovementDirection();
    }

    public void DeselectPlayer()
    {
        chooseAgent = null;
        chooseUnit = null;
    }
   
    private void DetectMouseInput()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && !TriggerBattle.startedBattle)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000) && hit.transform.CompareTag("Player"))
            {
                mouseOverPlayer = true;
            }
            else
            {
                mouseOverPlayer = false;
            }
        }
    }
    
    
    //Choose the unit
    private void ChosenUnit()
    {
        if (mouseOverPlayer && !DialogueManager.isInDialogue)
        {
            if (!TriggerBattle.startedBattle)
                if (Input.GetMouseButtonDown(0))
                {
                    chooseUnit = this.transform.gameObject;
                    chooseAgent = this.transform.gameObject.GetComponent<NavMeshAgent>();
                }
        }
        else
            if (Input.GetMouseButtonDown(0))
            {
                DeselectPlayer();
            }
    }
    
    private void WalkToPosition()
    {
        //Determines the position where the character should run in the world.
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        SceneManager.GetSceneByName("InGame_UI");
        
        if(chooseUnit)
            if(Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject()) 
                    Debug.Log("UI in way");
                else
                {
                    chooseAgent.SetDestination(hit.point);
                    lastPos = hit.point;
                }
            }
    }

     void FaceMovementDirection()
     {
         if (player.velocity.sqrMagnitude > Mathf.Epsilon)
         {
             this.transform.localRotation = Quaternion.LookRotation(player.velocity.normalized);
             metarig.transform.localRotation = Quaternion.Euler(-90, -90, 90);
         }
     }

     //Let players move to position
    void PlayerMoving()
    {
        if (Input.GetMouseButtonDown(1) && !TriggerBattle.startedBattle)
        {
            playerIsMoving = true;
            WalkToPosition();
        }
    }

    void DrawPath()
    {
        //Traces the player's path to the destination
        drawLine.positionCount = this.player.path.corners.Length;
        drawLine.SetPosition(0, transform.position);

        for (int i = 1; i < player.path.corners.Length; i++)
        {
            Vector3 pointPosition = new Vector3(player.path.corners[i].x, player.path.corners[i].y + 1.5f, player.path.corners[i].z);
            drawLine.SetPosition(i, pointPosition);
        }
    }

    public void ResetDrawLine()
    {
        drawLine.positionCount = 0;
    }
    
}
