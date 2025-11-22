using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;

    // ⭐ 외부에서 제어할 수 있는 스위치 추가
    public bool canMove = true;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ⭐ 스위치가 켜져 있을 때만 키보드 입력을 받음
        // 꺼져있으면 0이 되어 움직이지 않음
        float x = canMove ? Input.GetAxis("Horizontal") : 0;
        float z = canMove ? Input.GetAxis("Vertical") : 0;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // --- 중력 코드는 조건문 밖에서 항상 실행됨 (이제 공중부양 안 함!) ---
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}   