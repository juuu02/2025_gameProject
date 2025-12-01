using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float Damage = 20f;    // 보스 근접 공격 피해량
    private bool _canDamage = false;
    private bool _hasDealtDamage = false;

    // BossManager 또는 Boss_control에서 이걸 true/false로 제어함
    public void EnableDamage()
    {
        _canDamage = true;
        _hasDealtDamage = false;
    }

    public void DisableDamage()
    {
        _canDamage = false;
    }
    private void OnTriggerStay(Collider other)
    {
        // 1. 공격 타이밍이 아니거나, 이미 이번 공격에 피해를 입혔다면 무시
        if (!_canDamage || _hasDealtDamage) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log($"[DEBUG: TRIGGER] 호출! 닿은 대상: {other.gameObject.name}, Tag: {other.gameObject.tag}"); 
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(Damage);
                Debug.Log("💥 보스 공격 성공! (OnTriggerStay) 피해: " + Damage);

                // 🔥 한 번 피해를 입힌 후 바로 플래그 설정 (중복 피해 방지)
                _hasDealtDamage = true;
            }
            else
            {
                // 🔥 [추가] 태그는 'Player'인데 Health 컴포넌트가 없을 때
                Debug.LogError($"❌ [ERROR] Player Tag가 맞는데 PlayerHealth 컴포넌트가 없습니다! 오브젝트: {other.gameObject.name}");
            }
        }
    }

}
