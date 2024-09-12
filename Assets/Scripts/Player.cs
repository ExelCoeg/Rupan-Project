using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour, IDamagable
{
    Rigidbody rb;
    public int hitCount;
    public float speed;
    public PlayerInputActions playerControls;   
    public InputAction move;
    public InputAction attack;
    public InputAction interact;
    public InteractableObject currentInteractableObject;
    Animator anim;
    Vector3 moveDirection;
    RaycastHit hit;
    public Transform attackPoint;
    public float attackRadius;
    public int attackDamage;
    private string isWalkingString = "isWalking";
    private string attackString = "attack";
    public Transform spawnPoint;
    float hitCountTimer;
    public float hitCountResetTime;
    public bool enableDetectInteractableObject = true;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        playerControls = new PlayerInputActions();
    }
    private void Start() {
        transform.position = spawnPoint.position;
    }


    public void EnableControls(){
        print("EnableControls");
        move.Enable();
        attack.Enable();
        interact.Enable();
    }
    public void DisableControls(){
        print("DisableControls");
        move.Disable();
        attack.Disable();
        interact.Disable();
    }

    
    private void OnEnable() {
        move = playerControls.Player.Move;
        attack = playerControls.Player.Attack;
        interact = playerControls.Player.Interact;
        attack.performed += MeleeAttack;
        interact.performed += Interact;
        // interact.Enable();
        // attack.Enable();
        // move.Enable();
    }
    private void OnDisable() {
        move.Disable();
        attack.Disable();
        interact.Disable();
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
        moveDirection = moveDirection.x * transform.right + moveDirection.z * transform.forward;
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
            if(detectedInteractableObject != null){
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

    public void EnableDetect(){
        enableDetectInteractableObject = true;
        interact.Enable();
    }
    public void DisableDetect(){
        enableDetectInteractableObject = false;
        interact.Disable();
    }
    // public void EnableMovement(){
    //     move.Enable();
    // }
    // public void DisableMovement(){
    //     move.Disable();
    // }
    // public void DisableAttack(){
    //     attack.Disable();
    // }
    // public void EnableAttack(){
    //     attack.Enable();
    // }
}
