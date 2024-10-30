using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using System.Collections;
public class Player : MonoBehaviour, IDamagable
{
    // Rigidbody rb;
    Animator anim;
    Vector3 moveDirection;
    RaycastHit hit;
    [Header("On What Ground")]
    public GroundType onWhatGround;
    [Header("Player Stats")]
    public int hitCount;
    public float speed;
    public float currentSpeed;
    public float speedMultiplier = 1.5f;
    public float currentStamina;
    public float maxStamina = 10f;
    [Header("Player Input Actions")]
    public PlayerInputActions playerControls;   
    public InputAction move;
    public InputAction attack;
    public InputAction interact;
    public InputAction use;
    public InputAction sprintStart;
    public InputAction sprintFinish;
    public InputAction look;
    [Header("Interactable Object")]
    public InteractableObject currentInteractableObject;
    [Header("Detect Interactable Object")]
    
    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius;
    public int attackDamage;
    public float attackDelay = 0.01f;
    [SerializeField] private float attackDelayTimer;
    
    [Header("Hit Count Reset")]
    [SerializeField] private float hitCountTimer;
    [SerializeField] private float hitCountResetTime = 3f;

    [Header("Pickupable Object")]
    public Transform rightHand;
    public PickupableObject currentPickupableObject; 
    [Header("Player Timelines")]

    public List<PlayableDirector> playerTimelines;
    public Transform spawnPoint;
    [Header("Player Bools")]
    public bool isSprinting;
    [SerializeField] private bool enableMove = true;
    [SerializeField] private bool enableAttack = true;
    [SerializeField] private bool enableInteract = true;
    [SerializeField] private bool enableUse = true;
    [SerializeField] private bool enableSprint = true;
    [SerializeField] private bool gotHit = false;
    public bool enableDetectInteractableObject = true;
    private string isWalkingString = "isWalking";
    private string attackString = "attack";


    private void Awake() {
        // rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerControls = new PlayerInputActions();
    }
    private void Start() {
        // transform.position = spawnPoint.position;
        currentSpeed = speed;
        currentStamina = maxStamina;
        hitCountTimer = hitCountResetTime;
    }

    private void Update() {
        if(GameManager.instance.isPaused) return;

        attackDelayTimer -= Time.deltaTime;
        
        // hit -> timer jalan ke 0 -> kalau kena hit -> hitCount -= damage -> timer reset
       // hit -> timer jalan ke 0 -> kalau timer ke 0 -> hitCount = 3
        if(gotHit){
            // hitCountTimer -= Time.deltaTime;
            // if(hitCountTimer <= 0){
            //     ResetHitCount();
            // }
            StartCoroutine(TimedBooleanChange(hitCountResetTime, value => {
                ResetHitCount();
            }));
        }

        if(hitCount <= 0){
            PlayerRespawn();
        }

        if(enableDetectInteractableObject){
            DetectInteractableObject();
        }
        moveDirection = move.ReadValue<Vector3>();// movement

        anim.SetBool(isWalkingString,moveDirection != Vector3.zero);// anim
        
        // Player Stamina
        if(currentStamina > maxStamina){
            currentStamina = maxStamina;
        }
        else if(currentStamina < 0){
            currentStamina = 0;
        }
        if(isSprinting){
            currentStamina -= Time.deltaTime;

        }
        else{
            currentStamina += Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        moveDirection = moveDirection.x * -transform.right + moveDirection.z * -transform.forward;
        transform.position += moveDirection * currentSpeed * Time.fixedDeltaTime;
    }

    public void Attack(){
        anim.SetTrigger(attackString);
    }
    public void DetectAttack(){
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, LayerMask.GetMask("Enemy"));
        if(hitEnemies.Length > 0){
            foreach (Collider hit in hitEnemies)
            {
                if(hit.TryGetComponent(out IDamagable damagable)){
                    if(hit.TryGetComponent(out NPC enemyNPC)){
                        if(enemyNPC.state != NPC.State.FAINT){
                            damagable.TakeDamage(attackDamage);
                        }
                    }
                    SoundManager.instance.PlaySound2D("Punch");
                    return;
                }
            }   
        }
        attackDelayTimer = attackDelay;
    }

    private void Interact(InputAction.CallbackContext context){
        if(currentInteractableObject != null){
             print("Interact " + currentInteractableObject.name);
            currentInteractableObject.Interacted();
        }
    }

    private void Use(InputAction.CallbackContext context){
        if(currentPickupableObject != null && currentPickupableObject.isUsable){
            currentPickupableObject.Use();
        }
    }
    private void DetectInteractableObject(){
        if(currentInteractableObject != null){
            UIManager.instance.uiInteract.Show();
        }
        else{
            UIManager.instance.uiInteract.Hide();
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit, 1.5f,LayerMask.GetMask("Interactable"))){
            InteractableObject detectedInteractableObject = hit.collider.GetComponent<InteractableObject>();
            if(detectedInteractableObject != null && detectedInteractableObject.isInteractable){
                currentInteractableObject = detectedInteractableObject;
                currentInteractableObject.EnableOutline();
            }
        }
        else{
            if(currentInteractableObject != null){
                currentInteractableObject.DisableOutline();
                currentInteractableObject = null;
            }
        }
    }
    private void OnDrawGizmos() {
        if(attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void TakeDamage(int damage)
    {
        gotHit = true;
        Camera.main.DOShakeRotation(0.2f, 20, 10, 90, true);
        UIManager.instance.uiHitEffect.Activate();
        hitCount -= damage;
        hitCountTimer = hitCountResetTime;
    }
    public void ResetHitCount(){
        print("ResetHitCount");
        hitCount = 3;
        gotHit = false;
    }

    public void SetRightHandObject(PickupableObject pickupableObject){
        pickupableObject.GetComponent<Collider>().enabled = false;
        currentPickupableObject = pickupableObject;
        currentPickupableObject.gameObject.transform.SetParent(rightHand);
        currentPickupableObject.gameObject.transform.localPosition = Vector3.zero;
        currentPickupableObject.gameObject.transform.localRotation = rightHand.rotation;
        UIManager.instance.uiUse.UpdateText(currentPickupableObject.objectName);
    }

    public void RemoveRightHandObject(){
        if(currentPickupableObject != null){
            currentPickupableObject.ReturnToOriginalPosition();
            currentPickupableObject = null;
        }
        UIManager.instance.uiUse.Hide();
    }

    public void PlayTimeline(string timelineName){
        foreach (PlayableDirector timeline in playerTimelines)
        {
            if(timeline.name == timelineName){
                timeline.Play();
            }
        }
    }
    [ContextMenu("PlayerRespawn")]
    public void PlayerRespawn(){
        gotHit = false;
        hitCount = 3;
        hitCountTimer = hitCountResetTime;
        UIManager.instance.uiBlackScreen.IncreaseDay();
        UIManager.instance.uiBlackScreen.Show();
        PlayTimeline("PlayerWakeUp");
    }
    public void SprintPressed(){
        isSprinting = true;
        if(currentStamina <= 0){
            return;
        }
        currentSpeed *= speedMultiplier;
    }
    public void SprintReleased(){
        isSprinting = false;
        currentSpeed = speed;

    }
    public void EnableMove()
    {
        enableMove = true;
        move.Enable();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void EnableControls(){
        print("EnableControls");
        move.Enable();
        interact.Enable();
        use.Enable();
        look.Enable();
        attack.Enable();
    }
    public void DisableControls(){
        print("DisableControls");
        move.Disable();
        attack.Disable();
        interact.Disable();
        use.Disable();
        sprintFinish.Disable();
        sprintStart.Disable();
        look.Disable();
    }

    
    private void OnEnable() {
        move = playerControls.Player.Move;
        attack = playerControls.Player.Attack;
        interact = playerControls.Player.Interact;
        use = playerControls.Player.Use;
        sprintStart = playerControls.Player.SprintStart;
        sprintFinish = playerControls.Player.SprintFinish;
        look = playerControls.Player.Look;

        attack.performed += ctx =>{
            if(attackDelayTimer <= 0 && !GameManager.instance.isPaused){
                Attack();
            }
        };
        
        interact.performed += Interact;
        // interact.performed += ctx => {
        //     if(currentInteractableObject != null){
        //         SoundManager.instance.PlaySound2D("PlayerInteract");
        //     }
        // };

        sprintStart.performed += ctx => SprintPressed();
        sprintFinish.performed += ctx => SprintReleased();
        use.performed += Use;
    }
    private void OnDisable() {
        DisableControls();
    }

    public void EnableDetect(){
        enableDetectInteractableObject = true;
        interact.Enable();

    }
    public void DisableDetect(){
        enableDetectInteractableObject = false;
        interact.Disable();
    }
    public void EnableSprint(){
        enableSprint = true;
        sprintStart.Enable();
        sprintFinish.Enable();
        UIManager.instance.uiSprintBar.Show();
    }
    public void DisableSprint(){
        enableSprint = false;
        sprintStart.Disable();
        sprintFinish.Disable();
        UIManager.instance.uiSprintBar.Hide();
    }
    public void DisableMove()
    {
        enableMove = false;
        move.Disable();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    [ContextMenu("EnableAttack")]
    public void EnableAttack(){
        enableAttack =true;
        attack.Enable();
    }
    [ContextMenu("DisableAttack")]

    public void DisableAttack(){
        enableAttack =false;
        attack.Disable();
    }
    public void EnableCamera(){
        Camera.main.GetComponent<FPSCamera>().EnableCamera();
    }
    public void DisableCamera(){
        Camera.main.GetComponent<FPSCamera>().DisableCamera();
    }
    public void MakeCurrentInteractableObjectNull(){
        currentInteractableObject = null;
    }
    public void MakeCurrentPickupableObjectNull(){
        currentPickupableObject = null;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("WoodFloor")){
            onWhatGround = GroundType.WOOD;
        }
        else if(other.gameObject.CompareTag("GrassFloor")){
            onWhatGround = GroundType.GRASS;
        }
        else if(other.gameObject.CompareTag("GravelFloor")){
            onWhatGround = GroundType.GRAVEL;
        }
    }
    public void PlayAirPunchSound(){
        SoundManager.instance.PlaySound2D("Air-Punch");
    }


    private IEnumerator TimedBooleanChange(float duration, System.Action<bool> setBoolean)
    {
        yield return new WaitForSeconds(duration);
        setBoolean(true);
        Debug.Log("Boolean set to true after " + duration + " seconds");
    }
}


public enum GroundType{
    GRASS,
    GRAVEL,
    WOOD
}
