using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
public class FPSCamera : MonoBehaviour
{
    public Transform Target;
    public PlayerInputActions playerControls;
    public InputAction look;
    public float mouseSensitivity = 5f;
    private float verticalRotation;
    private float horizontalRotation;
    public bool disable;

    private void Awake() {
        playerControls = new PlayerInputActions();
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    

    void Update()
    {
        if(GameManager.instance.isPaused) return;
        float mouseX = look.ReadValue<Vector2>().x;
        float mouseY = look.ReadValue<Vector2>().y;

        verticalRotation -= mouseY * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -45f, 60f);

        horizontalRotation =  Target.rotation.eulerAngles.y +  mouseX * mouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation - 180, 0);
        Target.rotation = Quaternion.Euler(0, horizontalRotation , 0);
    }
    public void OnEnable(){
        look = playerControls.Player.Look;
        EnableCamera();
    }
    public void OnDisable(){
        DisableCamera();
    }

    public void EnableCamera(){
        look.Enable();
    }

    public void DisableCamera(){
        look.Disable();
    }

}