using UnityEngine;
public class FPSCamera : MonoBehaviour
{
    public Transform Target;
    public float mouseSensitivity = 5f;
    private float verticalRotation;
    private float horizontalRotation;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -20f, 20f);

        horizontalRotation = Target.rotation.eulerAngles.y +  mouseX * mouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        Target.rotation = Quaternion.Euler(0, horizontalRotation, 0);
    }
}