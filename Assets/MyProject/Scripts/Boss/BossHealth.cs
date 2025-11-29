using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Health")]
    public float MaxHP = 200f;
    public float HP = 200f;

    public delegate void BossDeathHandler();
    public event BossDeathHandler OnBossDeath;

    private bool _isDead = false;

    void Start()
    {
        HP = MaxHP;
    }

    public void TakeDamage(float dmg)
    {
        if (_isDead) return;

        HP -= dmg;
        Debug.Log($"🔥 Boss HP: {HP}");

        if (HP <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        Debug.Log("💀 Boss Dead!");

        OnBossDeath?.Invoke();

        gameObject.SetActive(false);
    }
}
