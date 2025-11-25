using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float health = 200f;

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        // 🔥 남은 HP 디버그 출력
        Debug.Log($"Boss HP: {health}");

        if (health <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log("Boss Dead!");
        gameObject.SetActive(false);
    }
}
