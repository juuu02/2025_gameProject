using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Coroutine 사용을 위해 추가

public class Boss_control : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movement")]
    public float speed = 5.0f;
    private Transform targetTransform; // 🔥 플레이어의 Transform을 받을 변수
    public bool canMove = false;

    public float attackStartDistance = 5.0f;

    [Header("Attack Settings")]
    public BossAttack attackTrigger;
    public float attackDelay = 1.5f;
    public float attackTriggerTime = 1f;
    public float attackDuration = 0.5f;
    public float attackAnimationLength = 1.0f;
    public float chaseDistance = 15f; // 플레이어를 추적하는 최대 거리

    private bool isAttacking = false;
    private bool isChasing = false; // 🔥 추적 상태 변수 추가

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed; // 에이전트 속도 설정

        agent.stoppingDistance = attackStartDistance;
    }

    // 🔥 Spawn_Boss에서 이 함수를 호출하여 플레이어 Transform을 설정합니다.
    public void SetTarget(Transform target)
    {
        targetTransform = target;
        isChasing = true; // 타겟이 설정되면 추적 시작
        Debug.Log("Boss_control: 플레이어 추적 시작.");
    }


    // Update에서 지속적으로 플레이어를 따라가도록 설정
    void Update()
    {
        if (!canMove || targetTransform == null || !isChasing)
            return;

        // 보스와 플레이어 사이의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        // 거리가 일정 범위 이내이면서 공격 중이 아닐 때만 이동
        if (!isAttacking && distanceToTarget > agent.stoppingDistance)
        {
            // NavMeshAgent의 목적지를 플레이어의 위치로 설정
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else if (!isAttacking && distanceToTarget <= agent.stoppingDistance)
        {
            // 공격 거리에 도달하면 멈추고 공격 시작
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
            StartAttackCycle();
        }
        else if (isAttacking)
        {
            // 공격 중일 때는 이동을 멈춥니다.
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
        }
    }

    // 🔥 공격 반복 루프
    private void StartAttackCycle()
    {
        if (!isAttacking)
        {
            // 기존 공격 루프가 실행 중이라면 다시 시작하지 않도록 방지
            StopAllCoroutines();
            StartCoroutine(AttackRoutine());
        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;
        Debug.Log("✅ [CONTROL] AttackRoutine 시작. isAttacking=true");

        // 보스가 플레이어를 바라보게 합니다.
        Vector3 lookPos = targetTransform.position - transform.position;
        lookPos.y = 0; // Y축 회전만 필요
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        // 공격 애니메이션이 끝날 때까지 무한 반복 (공격 거리 유지 시)
        while (Vector3.Distance(transform.position, targetTransform.position) <= agent.stoppingDistance + 0.5f)
        {
            float startTime = Time.time;

            // Attack 애니메이션 트리거
            animator.SetTrigger("Attack");
            Debug.Log("➡️ [CONTROL] 'Attack' 애니메이션 트리거 발동.");

            // 공격 타이밍까지 대기
            Debug.Log("⏳ [CONTROL] " + attackTriggerTime + "초 대기 시작.");
            yield return new WaitForSeconds(attackTriggerTime);

            // 데미지 ON
            attackTrigger.EnableDamage();
            Debug.Log("💥 [CONTROL] EnableDamage() 호출 완료! _canDamage=TRUE");

            // 짧은 데미지 적용 시간
            yield return new WaitForSeconds(attackDuration);

            // 데미지 OFF
            attackTrigger.DisableDamage();
            Debug.Log("⬇️ [CONTROL] DisableDamage() 호출 완료.");

            float timeSpent = Time.time - startTime;
            float timeToWait = attackAnimationLength - timeSpent;

            if (timeToWait > 0)
            {
                yield return new WaitForSeconds(timeToWait);
            }

            // 다음 공격까지 기다리기
            yield return new WaitForSeconds(attackDelay);
        }

        // 플레이어가 공격 사거리에서 벗어나면 추적 상태로 돌아감
        isAttacking = false;
    }
}