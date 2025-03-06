using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class Enemy : MonoBehaviour
{
    private bool chaseOn = false;
    private float preChase = 2.0f;
    private bool playerDetected = false;
    //private float detectTimer = 0.0f;
    private float turnTimer;

    private float moveSpeed = 3.5f;
    //private bool longWays = false;
    private float chaseVal;
    private Rigidbody rB;   

    //based upon the layer system in the editor
    public LayerMask targetMask; //will look for Player layer :3
    public LayerMask obstructionMask;

    public float detectionRadius = 4.0f;
    public float detectionAngle = 70.0f;

    //patrol mode variables
    public Transform[] patrolPoints;

    private NavMeshAgent enemyAgent;
    public Transform target;
    private Animator enemyAnimator;
    private int targetPoint;

    public GameObject player;




    void Start()
    {
        rB = GetComponent<Rigidbody>();
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        targetPoint = Random.Range(0, 6);
        enemyAgent.speed = 2.5f;

        enemyAgent.SetDestination(patrolPoints[targetPoint].position);
    }

    void Update ()
    {
        //check if player is in cone, if so start timer
        LookForPlayer();
        //once timer hits zero, set chaseOn true and call chasemode
        if (preChase <= 0)
        {
            ChaseMode(0);
            enemyAgent.SetDestination(target.position);
            if (chaseVal == 2){
                preChase = 2.0f;
            }
        }
        else {
            //Movement Patrol (Randomized)

            if (Vector3.Distance(enemyAgent.transform.position, patrolPoints[targetPoint].position) < 0.5f)
            {
                Debug.Log("Target hit, changing");
                targetPoint = changeTargetInt();
                SetPosition();
            }
        }
    }

    void SetPosition()
    {
        enemyAgent.SetDestination(patrolPoints[targetPoint].position);
    }
    int changeTargetInt()
    {
        int newVal = Random.Range(0,6);
        return newVal;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            enemyAgent.isStopped = true;
        }
    }

    private void LookForPlayer()
    {

        if (player == null)
        {
            return;
        }

        Vector3 enemyPosition = transform.position;

        Physics.CheckSphere(transform.position, detectionRadius, targetMask);

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if(Vector3.Dot(directionToTarget, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                //Vector3 toPlayer = PlayerMovement.Instance.transform.position - enemyPosition;
                //toPlayer.y = 0;
                float toPlayer = Vector3.Distance(player.transform.position, enemyPosition);

                if (!Physics.Raycast(transform.position, directionToTarget, toPlayer, obstructionMask)) 
                {
                    playerDetected = true;
                    if (playerDetected == true)
                    {
                        preChase = preChase - Time.deltaTime;
                        Debug.Log("" + preChase);
                    }
                    Debug.Log("player found");
                    
                }
                else
                    playerDetected = false;
            }
        else if (!playerDetected)
            playerDetected = false;

        if (!playerDetected)
            preChase = 2.0f;
    }


    void FixedUpdate()
    {
        //checks if player is in LoS cone (way thinner during chase mode), adds to chaseVal each time its unsuccessful'
        if (chaseOn){
            if (!playerDetected)
            {
                Debug.Log("Chase timer decreasing");
                chaseVal += Time.deltaTime;
                Debug.Log("Chase end timer =" + chaseVal);
                //if the tick hits 2 (since its fixed update), send back to chaseVal with a val of 2
                if (chaseVal >= 2)
                {
                    ChaseMode(2);
                }
            }
            else
            {
                chaseVal = 0;
            }
        }
        

    }
     
    void ChaseMode (int chaseVal)
    {
        Debug.Log("Timer hit 0, Chasing");
        //if chase is 0, turn up movespeed, change the cone angle and run at player via nav mesh
        if (chaseVal == 0){
            enemyAgent.speed = 5.5f;
            chaseOn = true;
            detectionAngle = 50.0f;
        }
        else if (chaseVal >= 2)
        {
            Debug.Log("Resetting");
            SetPosition();
            enemyAgent.speed = 2.5f;
            chaseOn = false;
            detectionAngle = 70.0f;
            preChase = 2.0f;
        }
        //once chaseval hits 2, call off chasemode (set every stat back to normal)
        
    }


}
