using UnityEngine;
using UnityEngine.AI;

public class Boss_control : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public float speed = 5.0f;

    public BossAttack attackTrigger;   // 🔥 공격 트리거 연결
    public float attackDelay = 1.5f;          // 공격 사이 딜레이
    public float attackTriggerTime = 0.3f;    // 공격 발동 타이밍
    public float attackDuration = 0.2f;       // 데미지 적용 시간

    private bool hasArrived = false;
    private bool isAttacking = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestinationTarget(Vector3 target)
    {
        if (agent != null)
            agent.SetDestination(target);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (hasArrived)
            return;

        GameObject other = coll.gameObject;

        if (other.CompareTag("RiverBarrier") || other.name == "RiverBarrier")
        {
            hasArrived = true;
            agent.isStopped = true;

            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            StartAttackCycle();     // 🔥 공격 시작
        }
    }

    // 🔥 공격 반복 루프
    private void StartAttackCycle()
    {
        if (!isAttacking)
            StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;

        while (true)  // 보스가 계속 공격함
        {
            // Attack 애니메이션 트리거
            animator.SetTrigger("Attack");

            // 공격 타이밍까지 대기 (애니메이션 타이밍과 맞추기)
            yield return new WaitForSeconds(attackTriggerTime);

            // 데미지 ON
            attackTrigger.EnableDamage();

            // 짧은 데미지 적용 시간
            yield return new WaitForSeconds(attackDuration);

            // 데미지 OFF
            attackTrigger.DisableDamage();

            // 다음 공격까지 기다리기
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
