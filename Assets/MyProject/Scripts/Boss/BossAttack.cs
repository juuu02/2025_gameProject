using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float Damage = 20f;    // 보스 근접 공격 피해량
    private bool _canDamage = false;

    // BossManager 또는 Boss_control에서 이걸 true/false로 제어함
    public void EnableDamage()
    {
        _canDamage = true;
    }

    public void DisableDamage()
    {
        _canDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canDamage) return;      // 공격 타이밍 아닐 때 무시

        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(Damage);
                Debug.Log("💥 보스 공격! " + Damage + " 피해");
            }
        }
    }
}
