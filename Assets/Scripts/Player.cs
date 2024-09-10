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
        if(GameManager.instance.isPaused) return;
        moveDirection = move.ReadValue<Vector3>();
        DetectInteractableObject();
    }
    private void FixedUpdate() {
        moveDirection = moveDirection.x * transform.right + moveDirection.z * transform.forward;
        rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    private void Fire(InputAction.CallbackContext context){
        print("Fire");
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
        
        if(Physics.Raycast(ray, out hit, 5f,LayerMask.GetMask("Interactable"))){
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
}
