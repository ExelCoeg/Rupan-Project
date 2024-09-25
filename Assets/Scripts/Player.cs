using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
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
    float hitCountTimer;
    public float hitCountResetTime;

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
    }

    private void Update() {
        if(GameManager.instance.isPaused) return;
        if(hitCount <= 0){
            GameManager.instance.GameOver();
        }

        hitCountTimer -= Time.deltaTime;
        if(hitCountTimer <= 0){
            ResetHitCountTimer();
        }

        attackDelayTimer -= Time.deltaTime;


        if(enableDetectInteractableObject){
            DetectInteractableObject();
        }
        moveDirection = move.ReadValue<Vector3>();
       
        anim.SetBool(isWalkingString,moveDirection != Vector3.zero);
        

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

    private void MeleeAttack(InputAction.CallbackContext context){
        if(attackDelayTimer > 0) return;
        anim.SetTrigger(attackString);
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, LayerMask.GetMask("Enemy"));
        foreach (IDamagable enemy in hitEnemies)
        {
            enemy.TakeDamage(attackDamage);
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
        
        if(Physics.Raycast(ray, out hit, 2f,LayerMask.GetMask("Interactable"))){
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
        Camera.main.DOShakeRotation(0.2f, 20, 10, 90, true);
        UIManager.instance.uiHitEffect.Activate();
        hitCount -= damage;
    }
    public void ResetHitCountTimer(){
        hitCountTimer = hitCountResetTime;
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

    public void SprintPressed(){
        isSprinting = true;
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
    }
    public void DisableControls(){
        print("DisableControls");
        move.Disable();
        attack.Disable();
        interact.Disable();
        use.Disable();
        sprintFinish.Disable();
        sprintStart.Disable();

    }

    
    private void OnEnable() {
        move = playerControls.Player.Move;
        attack = playerControls.Player.Attack;
        interact = playerControls.Player.Interact;
        use = playerControls.Player.Use;
        sprintStart = playerControls.Player.SprintStart;
        sprintFinish = playerControls.Player.SprintFinish;

        attack.performed += MeleeAttack;
        interact.performed += Interact;
        interact.performed += ctx => SoundManager.instance.PlaySound2D("PlayerInteract");

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
}

public enum GroundType{
    GRASS,
    GRAVEL,
    WOOD
}
