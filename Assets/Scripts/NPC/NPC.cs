using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class NPC : MonoBehaviour,IDamagable
{
    public enum State
    {
        IDLE,
        PATROLING,
        CHASE,
        ATTACK,
        HIT,
        FAINT
    }

    [Header("General")]
    [SerializeField] State state;
    [Space]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Player player;

    [Header("Movement System")]
    [SerializeField] float movSpeed;
    [SerializeField] float movSpeedRun;
    [Space]
    [SerializeField] float movCurrentSpeed;

    [Header("Walkpoint System")]
    [SerializeField] Transform SpawnPoint;
    [SerializeField] List<Transform> WalkPointList;
    [Space]
    [SerializeField] Vector3 currentWalkPoint;
    [SerializeField] bool isWalkPointSet;

    [Header("Detection System")]
    public Transform playerTransform;
    public float detectionRadius;
    public float detectionAngle;
    [SerializeField] LayerMask obstructionMask;
    public bool canSeePlayer;

    [Header("Idle System")]
    [SerializeField] float minIdleDelay;
    [SerializeField] float maxIdleDelay;
    [SerializeField] float lastIdleTime;
    [SerializeField] float scanningAngle;
    [SerializeField] float scanningSpeed;
    [SerializeField] bool isScanning;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool rotatingToEnd;
    private float timeElapsed;

    [Header("Attack System")]
    [SerializeField] float damage;
    [SerializeField] float attackSpeed;//based on second
    [SerializeField] float rangeAttack;
    [SerializeField] float lastAttackTime;
    [SerializeField] bool isAlreadyAttack;

    // Posisi terakhir player terlihat
    private Vector3 lastKnownPlayerPosition;

    // Scanning rotation state
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movSpeed;
        player = FindAnyObjectByType<Player>();
        playerTransform = player.transform;
    }

    void Update()
    {
        Detection();
        switch (state)
        {
            case State.IDLE:
                IdleState();
                break;
            case State.PATROLING:
                PatrolingState();
                break;
            case State.CHASE:
                ChaseState();
                break;
            case State.ATTACK:
                AttackState();
                break;
            case State.FAINT:
                FAINT();
                break;
        }
        FieldOfViewCheck();

        if (Vector3.Distance(transform.position, currentWalkPoint) < 1f)
        {
            isWalkPointSet = false;
        }
    }

    public void Detection()
    {
        switch (state)
        {
            case State.IDLE:
            case State.PATROLING:
                if (canSeePlayer)
                {
                    ChangeState(State.CHASE);
                }
                break;
            case State.CHASE:
                if (!canSeePlayer && isWalkPointSet)
                {
                    SetDestination(lastKnownPlayerPosition);
                }
                if (!isWalkPointSet)
                {
                    ChangeState(State.PATROLING);
                }

                if (Vector3.Distance(transform.position, playerTransform.position) < rangeAttack)
                {
                    ChangeState(State.ATTACK);
                }
                break;
            case State.ATTACK:
                if (!canSeePlayer)
                {
                    ChangeState(State.IDLE);
                }
                if (Vector3.Distance(transform.position, playerTransform.position) > rangeAttack)
                {
                    ChangeState(State.CHASE);
                }
                break;
            case State.FAINT:
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (newState != State.IDLE && state == State.IDLE)
        {
            lastIdleTime = Time.time;
        }
        state = newState;
    }

    public void IdleState()
    {
        SetDestination(transform.position);
        if (!isScanning)
        {
            Debug.Log("Scanning run");
            isScanning = true;

            startRotation = transform.rotation;
            bool rotateRight = Random.Range(0, 2) == 0;
            endRotation = Quaternion.Euler(transform.eulerAngles + (rotateRight ? Vector3.up * scanningAngle : Vector3.up * -scanningAngle));

            rotatingToEnd = true;
            timeElapsed = 0f;
        }
        if (isScanning)
        {
            Scanning();
        }
    }

    public void PatrolingState()
    {
        if (!isWalkPointSet)
        {
            SetDestination(GetRandomWalkPoint());
        }

        float randomIdleDelay = Random.Range(minIdleDelay, maxIdleDelay);

        if (Time.time - lastIdleTime > randomIdleDelay)
        {
            ChangeState(State.IDLE);
        }
    }

    public void ChaseState()
    {
        if (canSeePlayer)
        {
            lastKnownPlayerPosition = playerTransform.position;  // Update lokasi terakhir player terlihat
            SetDestination(playerTransform.position);
        }
    }

    public void AttackState()
    {
        SetDestination(transform.position);
        if (!isAlreadyAttack && (Time.time - lastAttackTime) > attackSpeed)
        {
            Debug.Log("attackStart");
            Invoke(nameof(Attack), 1f);
            isAlreadyAttack = true;
        }
    }

    public void FAINT() { }

    public void SetDestination(Vector3 targetWalkPoint)
    {
        agent.SetDestination(targetWalkPoint);
        currentWalkPoint = targetWalkPoint;
        isWalkPointSet = true;
    }

    public void Attack()
    {
        Debug.Log("attack running");
        lastAttackTime = Time.time;
        isAlreadyAttack = false;
    }
    public void TakeDamage(int damage)
    {
        
    }

    private void Scanning()
    {
        if (canSeePlayer)
        {
            // Instantly stop scanning if player is detected
            isScanning = false;
            ChangeState(State.CHASE);
            return;
        }

        timeElapsed += Time.deltaTime;

        // Rotate NPC either towards endRotation or back to startRotation
        if (rotatingToEnd)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / scanningSpeed);

            if (timeElapsed >= scanningSpeed)
            {
                // After reaching the end rotation, start rotating back
                rotatingToEnd = false;
                timeElapsed = 0f;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(endRotation, startRotation, timeElapsed / scanningSpeed);

            if (timeElapsed >= scanningSpeed)
            {
                // Once the rotation is back to the start, stop scanning and resume patroling
                isScanning = false;
                ChangeState(State.PATROLING);
            }
        }
    }

    public Vector3 GetRandomWalkPoint()
    {
        Vector3 newWalkPoint = WalkPointList[Random.Range(0, WalkPointList.Count - 1)].position;
        return newWalkPoint;
    }

    public void FieldOfViewCheck()
    {
        Vector3 directionToTarget = (playerTransform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
        if (Vector3.Angle(transform.forward, directionToTarget) < detectionAngle / 2 && distanceToTarget <= detectionRadius)
        {
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                canSeePlayer = true;
                lastKnownPlayerPosition = playerTransform.position; // Update lokasi terakhir player terlihat
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }

}
