using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private bool chaseOn = false;
    private float preChase = 2.0f;
    private bool playerDetected = false;
    //private float detectTimer = 0.0f;
    private float turnTimer;

    private float moveSpeed = 3.5f;
    //private bool longWays = false;
    private int chaseVal = -1;
    private Rigidbody rB;   

    //based upon the layer system in the editor
    public LayerMask targetMask; //will look for Player layer :3
    public LayerMask obstructionMask;

    public float detectionRadius = 30.0f;
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
        targetPoint = 0;
    }

    void Update ()
    {
        //check if player is in cone, if so start timer
        LookForPlayer();
        //once timer hits zero, set chaseOn true and call chasemode
        if (preChase == 0)
        {
            ChaseMode(0);
            enemyAgent.destination = target.position;
            if (chaseVal == 2){
                preChase = 2.0f;
            }
        }
        else {
            //Movement Path
            /*
            //walk with a timer, after x seconds turn right
            turnTimer -= Time.deltaTime;
            rB.MovePosition(this.transform.position + this.transform.forward * moveSpeed * Time.deltaTime); 
            if (turnTimer == 0){
                rB.MovePosition(this.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self));
                if (longWays){
                    turnTimer = 5.0f;
                    longWays = false;
                }
                else{
                     turnTimer = 2.0f;
                     longWays = true;
                }
            }
            */

            //Movement Patrol (Randomized)
            if (transform.position == patrolPoints[targetPoint].position)
            {
                targetPoint = changeTargetInt();
            }
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, moveSpeed * Time.deltaTime);
        }
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

        if(Vector3.Angle(transform.forward, directionToTarget) < detectionAngle / 2)
            {
                //Vector3 toPlayer = PlayerMovement.Instance.transform.position - enemyPosition;
                //toPlayer.y = 0;
                float toPlayer = Vector3.Distance(player.transform.position, enemyPosition);

                if (Physics.Raycast(transform.position, directionToTarget, toPlayer, obstructionMask)) 
                {
                    playerDetected = true;
                    Debug.Log("player found");
                    preChase -= Time.deltaTime;
                }
                else
                    playerDetected = false;
            }
        else if (!playerDetected)
            playerDetected = false;
        //did not account for obstruction
        /*
        Vecter3 enemyPosition = transform.position;
        Vector3 toPlayer = PlayerMovement.Instance.transform.position - enemyPosition;
        toPlayer.y = 0;

        if (toPlayer.magnitude <= detectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                playerDetected = true;
                preChase -= Time.deltaTime;

                return Player.Instance;
            } 
            
        }

        return null; */
    }


    void FixedUpdate()
    {
        //checks if player is in LoS cone (way thinner during chase mode), adds to chaseVal each time its unsuccessful'
        if (chaseOn){
            if (!playerDetected)
            {
                chaseVal += (int)Time.deltaTime;
                //if the tick hits 2 (since its fixed update), send back to chaseVal with a val of 2
                if (chaseVal == 2)
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
        //if chase is 0, turn up movespeed, change the cone angle and run at player via nav mesh
        if(chaseVal == 0){
            moveSpeed = 5.5f;
            chaseOn = true;
            detectionAngle = 50.0f;
        }
        else if (chaseVal == 2)
        {
            moveSpeed = 3.5f;
            chaseOn = false;
            detectionAngle = 70.0f;
        }
        //once chaseval hits 2, call off chasemode (set every stat back to normal)
        
    }


}
