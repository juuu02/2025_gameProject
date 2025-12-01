using UnityEngine;
using UnityEngine.AI;

public class CowBoy_control : MonoBehaviour
{
    [Header("Components")]
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Target Info")]
    private bool hasArrived = false;
    private Vector3 targetPosition;
    private EnemyManager _enemyManager;

    [Header("Speed Settings (속도 조절)")]
    [Tooltip("EnemyManager가 계산한 속도에 곱해질 값입니다. (0.5 = 절반 속도, 1.0 = 그대로)")]
    [Range(0.1f, 1.0f)]
    public float SpeedMultiplier = 0.5f; // 👈 기본적으로 속도를 절반으로 줄임 (조절 가능)

    [Tooltip("적이 낼 수 있는 최대 속도 제한입니다.")]
    public float MaxSpeedLimit = 3.5f; // 👈 아무리 빨라도 3.5 이상은 못 가게 막음

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 게임 오버 호출을 위해 매니저 찾아두기
        _enemyManager = FindFirstObjectByType<EnemyManager>();
    }

    // ------------------------------------------------------
    // [Step 1] spawn_enemy에서 호출: 목표 위치만 기억
    // ------------------------------------------------------
    public void SetDestinationTarget(Vector3 target)
    {
        targetPosition = target;
        // 아직 출발하지 않음 (속도 0 상태 혹은 NavMeshAgent 멈춤 상태)
        if (agent != null)
        {
            agent.speed = 0f;
        }
    }

    // ------------------------------------------------------
    // [Step 2] EnemyManager에서 호출: 타겟 위치 반환
    // ------------------------------------------------------
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    // ------------------------------------------------------
    // [Step 3] EnemyManager에서 호출: 계산된 속도를 받아서 적용 (감속 로직 추가됨)
    // ------------------------------------------------------
    public void SetMoveSpeed(float calculatedSpeed)
    {
        if (agent != null)
        {
            // 1. 매니저가 계산해준 속도에 '감속 비율'을 곱합니다.
            float finalSpeed = calculatedSpeed * SpeedMultiplier;

            // 2. 만약 그래도 너무 빠르면 '최대 속도'로 고정합니다.
            if (finalSpeed > MaxSpeedLimit)
            {
                finalSpeed = MaxSpeedLimit;
            }

            // 3. 최종 속도 적용 및 출발
            agent.speed = finalSpeed;
            agent.SetDestination(targetPosition);

            Debug.Log($"🏃 {gameObject.name} 속도 설정 완료: 계산됨({calculatedSpeed:F1}) -> 최종 적용({finalSpeed:F1})");

            // (선택) 애니메이션 속도도 이동 속도에 맞춰 자연스럽게 조절 (걷는 모션이 스케이트 타는 것처럼 보이지 않게)
            if (animator != null)
            {
                // 이동 속도가 3.5일 때 애니메이션 속도 1배속이 되도록 비율 조정
                animator.speed = finalSpeed / 3.5f;
            }
        }
    }

    // (참고) spawn_enemy와 EnemyManager가 동시에 설정할 때 쓰는 통합 함수
    public void SetupEnemy(Vector3 target, float moveSpeed)
    {
        targetPosition = target;
        SetMoveSpeed(moveSpeed); // 위의 SetMoveSpeed 로직을 태움
    }

    // ------------------------------------------------------
    // 강가 도착 처리
    // ------------------------------------------------------
    private void OnCollisionEnter(Collision coll)
    {
        if (hasArrived) return;

        GameObject other = coll.gameObject;

        if (other.CompareTag("RiverBarrier") || other.name == "RiverBarrier")
        {
            hasArrived = true;
            if (agent != null) agent.isStopped = true;

            if (animator != null)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                animator.SetBool("Fire", true);

                // 애니메이션 속도 원상복구 (사격 동작은 정상 속도로)
                animator.speed = 1.0f;

                // 🚨 게임 오버 신호 보냄
                if (_enemyManager != null)
                    _enemyManager.OnEnemyArrivedAtRiver();
            }
        }
    }
}