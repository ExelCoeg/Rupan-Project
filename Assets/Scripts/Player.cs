using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public PlayerInputActions playerControls;   
    public InputAction move;
    public InputAction fire;
    public InputAction interact;
    public InteractableObject currentInteractableObject;
    Vector3 moveDirection;
    RaycastHit hit;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerInputActions();
    }
    private void OnEnable() {
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        interact = playerControls.Player.Interact;
        fire.performed += Fire;
        interact.performed += Interact;
        interact.Enable();
        fire.Enable();
        move.Enable();
    }
    private void OnDisable() {
        move.Disable();
        fire.Disable();
        interact.Disable();
    }
    private void Update() {
        moveDirection = move.ReadValue<Vector3>();
        PlayerInteract();
    }
    private void FixedUpdate() {
        moveDirection = moveDirection.x * transform.right + moveDirection.z * transform.forward;
        rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
    }

    private void Fire(InputAction.CallbackContext context){
        print("Fire");
    }

    private void Interact(InputAction.CallbackContext context){
        print("Interact");
    }

    private void PlayerInteract(){
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(transform.position,transform.forward, out hit, 2f,LayerMask.GetMask("Interactable"))){
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

        if(Input.GetKeyDown(KeyCode.F) && currentInteractableObject != null){
            // UIManager.instance.UIInteractText.Show();
            currentInteractableObject.Interacted();
        }
    }
}
