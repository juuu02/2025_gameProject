using UnityEngine;
using UnityEngine.AI;

public class CowBoy_control : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float speed = 5.0f;
    private bool hasArrived = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        float targetXCenter = (-21.0f + 21.0f) / 2f;
        Vector3 targetPosition = new Vector3(targetXCenter, 2.0f, -50.0f);
        agent.SetDestination(targetPosition);
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
                animator.SetBool("Fire", true);
            }
        }
    }
}
