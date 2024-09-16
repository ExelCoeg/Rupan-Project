using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
public class Player : MonoBehaviour, IDamagable
{
    Rigidbody rb;
    Animator anim;
    Vector3 moveDirection;
    RaycastHit hit;
    [Header("On What Ground")]
    public GroundType onWhatGround;
    public int hitCount;
    public float speed;
    public Transform spawnPoint;
    public PlayerInputActions playerControls;   
    [Header("Player Input Actions")]
    public InputAction move;
    public InputAction attack;
    public InputAction interact;
    public InputAction use;
    [Header("Interactable Object")]
    public InteractableObject currentInteractableObject;
    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius;
    public int attackDamage;
    [Header("Hit Count Reset")]
    float hitCountTimer;
    public float hitCountResetTime;
    [Header("Detect Interactable Object")]
    public bool enableDetectInteractableObject = true;

    [Header("Right Hand")]
    public Transform rightHand;
    public PickupableObject currentPickupableObject; 
    [Header("Player Timelines")]

    public List<PlayableDirector> playerTimelines;
    
    private string isWalkingString = "isWalking";
    private string attackString = "attack";


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerControls = new PlayerInputActions();
    }
    private void Start() {
        // transform.position = spawnPoint.position;
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


        if(enableDetectInteractableObject){
            DetectInteractableObject();
        }
        moveDirection = move.ReadValue<Vector3>();
       
        anim.SetBool(isWalkingString,moveDirection != Vector3.zero);
        
    }
    private void FixedUpdate() {
        moveDirection = moveDirection.x * -transform.right + moveDirection.z * -transform.forward;
        rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    private void MeleeAttack(InputAction.CallbackContext context){
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, LayerMask.GetMask("Enemy"));
        foreach (IDamagable enemy in hitEnemies)
        {
            enemy.TakeDamage(attackDamage);
        }
        anim.SetTrigger(attackString);
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
    public void EnableMove()
    {
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
    }

    
    private void OnEnable() {
        move = playerControls.Player.Move;
        attack = playerControls.Player.Attack;
        interact = playerControls.Player.Interact;
        use = playerControls.Player.Use;
        attack.performed += MeleeAttack;
        interact.performed += Interact;
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
    public void DisableMove()
    {
        move.Disable();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void EnableAttack(){
        attack.Enable();
    }
    public void DisableAttack(){
        attack.Disable();
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
