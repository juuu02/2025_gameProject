using UnityEngine;
using UnityEngine.AI;

public class Boss_control : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float speed = 5.0f;
    private bool hasArrived = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestinationTarget(Vector3 target)
    {
        if (agent != null)
        {
            agent.SetDestination(target);
        }
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

            if (animator != null)
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                animator.SetBool("Attack", true);
            }
        }
    }
}
