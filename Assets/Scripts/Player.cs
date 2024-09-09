using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    Vector3 move;
    Rigidbody rb;

    public float speed;
    public InputAction playerControls;   
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    private void Update() {
        move = playerControls.ReadValue<Vector3>();
    }
    private void FixedUpdate() {
            move = move.x * transform.right + move.z * transform.forward;
         rb.MovePosition(transform.position + move * speed * Time.deltaTime);
    }
}
