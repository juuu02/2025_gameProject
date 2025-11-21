using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 마우스 입력 (Raw 추천)
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 상하 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  

        // 좌우 회전 — 여기 deltaTime만 적용
        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);
    }
}