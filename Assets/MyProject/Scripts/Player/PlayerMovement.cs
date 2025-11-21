using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        // Player 방향 기준으로 움직임
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 지면 체크
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;   // 바닥에 붙이는 역할만 남김
        }

        // 점프 제거됨
        // if (Input.GetButtonDown("Jump") && controller.isGrounded)
        // {
        //     velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        // }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
