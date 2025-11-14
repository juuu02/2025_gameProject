using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150f;

    CharacterController controller;
    Transform cam;

    float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;   // 마우스 가운데 고정
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");  // A, D
        float z = Input.GetAxis("Vertical");    // W, S

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 좌우 회전(몸)
        transform.Rotate(Vector3.up * mouseX);

        // 상하 회전(카메라만)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);  // 고개 너무 꺾임 방지

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
