using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{

    public float mouseSensitivity = 50f;
    public Transform cam;
    private float xRotation = 0f;
    private Vector2 lookInput;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void HandleMouseLook()
    {

        Debug.Log("Look Input: " + lookInput);
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        lookInput = Vector2.zero;
    }
}
