using UnityEngine;
using UnityEngine.AI;

public class CowBoy_control : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float speed = 5.0f;
    private float targetXMin = -21.0f;
    private float targetXMax = 21.0f;
    private float targetY = 2.0f;
    private float targetZ = -18.0f;
    private float yTolerance = 1.0f;
    private float zTolerance = 0.5f;
    private bool hasArrived = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        float targetXCenter = (targetXMin + targetXMax) / 2f;
        Vector3 targetPosition = new Vector3(targetXCenter, 2.0f, targetZ);
        agent.SetDestination(targetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasArrived)
            return;

        if (IsInRange())
        {
            hasArrived = true;
            agent.isStopped = true;

            if (animator != null)
            {
                animator.SetBool("Fire", true);
            }
        }
    }

    bool IsInRange()
    {
        bool inXRange = transform.position.x >= targetXMin &&
                        transform.position.x <= targetXMax;
        bool inYRange = Mathf.Abs(transform.position.y - targetY) <= yTolerance;
        bool inZRange = Mathf.Abs(transform.position.z - targetZ) <= zTolerance;
        bool nearTarget = agent.remainingDistance < 5f; 

        return nearTarget && inXRange && inYRange && inZRange;
    }
}
