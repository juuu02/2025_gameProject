using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;

    // 외부에서 제어할 수 있는 스위치
    public bool canMove = true;

    CharacterController controller;
    Vector3 velocity;

    // 발자국 관련
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float footstepInterval = 0.45f;
    float footstepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleFootsteps();
    }

    void HandleMovement()
    {
        float x = canMove ? Input.GetAxis("Horizontal") : 0;
        float z = canMove ? Input.GetAxis("Vertical") : 0;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

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

    void HandleFootsteps()
    {
        // 이동 속도 계산 (중력 제외)
        Vector3 horizontalVel = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        bool isMoving = horizontalVel.magnitude > 0.1f;

        // 움직일 때만 발자국 작동
        if (isMoving && controller.isGrounded && canMove)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval)
            {
                footstepSource.PlayOneShot(footstepClip);
                footstepTimer = 0f;
            }
        }
        else
        {
            // 멈추면 타이머 리셋 -> 소리 즉시 멈춤
            footstepTimer = footstepInterval;
        }
    }
}
