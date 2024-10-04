using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private Material hitMaterial;
    [Header("General")]
    public State state;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Player player;
    [SerializeField] bool isHealed;

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
    public float detectionRadius;
    public float detectionAngle;
    [SerializeField] LayerMask obstructionMask;
    [Space]
    public Transform playerTransform;
    public bool canSeePlayer;

    [Header("Idle System")]
    [SerializeField] float minIdleDelay;
    [SerializeField] float maxIdleDelay;
    [SerializeField] float lastIdleTime;
    [SerializeField] float scanningAngle;
    [SerializeField] float scanningSpeed;
    [Space]
    [SerializeField] bool isScanning;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool rotatingToEnd;
    private float timeElapsed;

    [Header("Attack System")]
    [SerializeField] float damage;
    [SerializeField] float attackSpeed;//based on second
    [SerializeField] float rangeAttack;
    [Space]
    [SerializeField] float lastAttackTime;
    [SerializeField] bool isAlreadyAttack;
    [Header("Health System")]
    [SerializeField] float maxHealth = 3;
    [Space]
    [SerializeField] float currentHealth = 3;

    // Posisi terakhir player terlihat
    private Vector3 lastKnownPlayerPosition;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string idleAnim;
    [SerializeField] private string walkAnim;
    [SerializeField] private string runAnim;
    [SerializeField] private string attackAnim;
    [SerializeField] private string fallAnim;
    [SerializeField] private string standUpAnim;
    [SerializeField] private string healedAnim;
    

    // Scanning rotation state
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movSpeed;
        player = FindAnyObjectByType<Player>();
        playerTransform = player.transform;
        ChangeState(State.PATROLING);
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
        switch (state)
        {
            case State.IDLE:
                animator.CrossFade(idleAnim, 0.1f);
                isAlreadyAttack = false;
                break;
            case State.PATROLING:
                movCurrentSpeed = movSpeed;
                animator.CrossFade(walkAnim, 0.1f);
                isAlreadyAttack = false;
                break;
            case State.CHASE:
                movCurrentSpeed = movSpeedRun;
                animator.CrossFade(runAnim, 0.1f);
                isAlreadyAttack = false;
                break;
            case State.ATTACK:
                break;
            case State.FAINT:
                animator.CrossFade(fallAnim, 0.1f);
                isAlreadyAttack = false;
                FAINT();
                break;
            case State.HIT:
                break;
        }
        agent.speed = movCurrentSpeed;
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
            animator.CrossFade(attackAnim,0.1f,0,0);
            isAlreadyAttack = true;
        }
    }

    public void FAINT() 
    { 
        SetDestination(transform.position);
        StartCoroutine(Revive(5));
    }

    public IEnumerator Revive(float delayTime)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null in Revive()");
        }

        if (NPCManager.instance == null)
        {
            Debug.LogError("NPCManager.instance is null in Revive()");
        }

        yield return new WaitForSeconds(delayTime);
        animator.CrossFade(standUpAnim,0.1f);
        currentHealth = maxHealth;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
            }
        }
        NPCManager.instance.HealNPC();
        isHealed = true;
        animator.CrossFade(healedAnim,0.1f);
    }
    public void RunOutOfWord(){
        Debug.Log("TO out word");
        agent.speed = 3;
        SetDestination(NPCManager.instance.GetRandomOutPoint());
    }

    public void SetDestination(Vector3 targetWalkPoint)
    {
        agent.SetDestination(targetWalkPoint);
        currentWalkPoint = targetWalkPoint;
        isWalkPointSet = true;
    }

    public void Attack()
    {
        if (Vector3.Distance(transform.position,playerTransform.position) < rangeAttack)
        {
            player.TakeDamage((int)damage);
        }
        lastAttackTime = Time.time;
        isAlreadyAttack = false;
    }
    public void TakeDamage(int damage)
    {
        if(state == State.FAINT) return;
        Debug.Log("Zombie take damage");
        if (currentHealth == 0)
        {
            ChangeState(State.FAINT);
        }
        hitMaterial.DOVector(new Vector3(10,10,10),"_Intensity",0.05f).OnComplete(()=>{
            hitMaterial.DOVector(new Vector3(1,1,1),"_Intensity",0.5f);
            currentHealth -= damage;
            });
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

  
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position,rangeAttack);
    }
}
