using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialEnemy : MonoBehaviour
{
    public float preDeathSprint = 4.0f;
    private bool isCooked;
    private bool chaseOn = false;
    private float preChase = 2.0f;
    private bool playerDetected = false;
    //private float detectTimer = 0.0f;
    private float turnTimer;

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
    public GameObject tutorialEnemy;
    PlayerMovement playerS;

    //sound variables
    AudioSource audioSource;
    public AudioClip footstepsWalk;
    public AudioClip footstepsRun;
    public AudioClip alertNoise;
    public AudioClip chaseMusic;
    public AudioClip chaseEscape;
    private bool walking;
    private bool running;

    public int startingPoint;


    void Start()
    {
        rB = GetComponent<Rigidbody>();
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        targetPoint = Random.Range(0, patrolPoints.Length);
        startingPoint = targetPoint;
        enemyAgent.speed = 1.5f;
        playerS = player.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        enemyAgent.SetDestination(patrolPoints[targetPoint].position);
        audioSource.loop = true;
    }

    void Update()
    {
        //check if player is in cone, if so start timer
        LookForPlayer();
        //CheckForKey();

        //once timer hits zero, set chaseOn true and call chasemode
        if (preChase <= 0)
        {
            ChaseMode(0);
            enemyAgent.SetDestination(target.position);
            if (chaseVal == 2)
            {
                preChase = 2.0f;
            }
        }
        else
        {
            //Movement Patr ol (Randomized)

            if (Vector3.Distance(enemyAgent.transform.position, patrolPoints[targetPoint].position) < 0.5f)
            {
                Debug.Log("Target hit, changing");
                targetPoint = changeTargetInt();
                SetPosition();
            }
        }
    }

    void soundSpeedController()
    {
        if (walking)
        {
            PlaySoundOnce(footstepsWalk);
        }
        else if (running)
        {
            PlaySoundOnce(footstepsRun);
        }
    }
    /*void CheckForKey()
    {
        if (playerS.hasKey)
        {
            Instantiate(enemy, patrolPoints[targetPoint].position, transform.rotation);
            walking = true;
        }
    }*/
    void SetPosition()
    {
        enemyAgent.SetDestination(patrolPoints[targetPoint].position);
    }
    int changeTargetInt()
    {
        int newVal = Random.Range(1, 6);
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

        Physics.CheckSphere(transform.position, 4.0f);

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Vector3.Dot(directionToTarget, transform.forward) > Mathf.Cos(70.0f * 0.5f * Mathf.Deg2Rad))
        {
            //Vector3 toPlayer = PlayerMovement.Instance.transform.position - enemyPosition;
            //toPlayer.y = 0;
            float toPlayer = Vector3.Distance(player.transform.position, enemyPosition);

            if (!Physics.Raycast(transform.position, directionToTarget, toPlayer))
            {
                playerDetected = true;
                if (playerDetected == true)
                {
                    preChase = preChase - Time.deltaTime;
                    Debug.Log("Pre-chase timer:" + preChase);
                }
                Debug.Log("player found");
                audioSource.loop = false;
                PlaySoundOnce(alertNoise);
                audioSource.loop = true;

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
        if (chaseOn && !isCooked)
        {
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

    public void ChaseMode(int chaseVal)
    {

        //if chase is 0, turn up movespeed, change the cone angle and run at player via nav mesh
        if (chaseVal == 0)
        {
            Debug.Log("Timer hit 0, Chasing");
            PlaySoundOnce(chaseMusic);
            enemyAgent.speed = 5.5f;
            chaseOn = true;
        }
        else if (chaseVal == 2)
        {
            Debug.Log("Resetting");
            SetPosition();
            audioSource.loop = false;
            PlaySoundOnce(chaseEscape);
            audioSource.loop = true;
            enemyAgent.speed = 2.5f;
            chaseOn = false;
        }
        //once chaseval hits 2, call off chasemode (set every stat back to normal)

    }

    public void PlaySoundOnce(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
