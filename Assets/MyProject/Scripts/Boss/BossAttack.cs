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
    //private void OnTriggerEnter(Collider other)
    //{
    //    // 🔥 1. 공격 범위 진입 시점을 확인
    //    Debug.Log($"[BossAttack] Collider Entered: {other.gameObject.name} (CanDamage: {_canDamage})");

    //    if (!_canDamage) return;

    //    // 🔥 2. 플레이어 태그 일치 여부 확인
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("[BossAttack] Player Tag Matched. Trying to get PlayerHealth.");

    //        PlayerHealth hp = other.GetComponent<PlayerHealth>();
    //        if (hp != null)
    //        {
    //            hp.TakeDamage(Damage);
    //            Debug.Log("💥 보스 공격 성공! " + Damage + " 피해"); // 성공 메시지
    //        }
    //        else
    //        {
    //            // 🔥 3. PlayerHealth 컴포넌트가 없는 경우를 확인
    //            Debug.LogError("❌ [BossAttack] Player Tag는 맞지만, PlayerHealth 컴포넌트를 찾을 수 없습니다!");
    //        }
    //    }
    //}
    private void OnTriggerStay(Collider other)
    {
        // 1. 공격 타이밍이 아니거나, 이미 이번 공격에 피해를 입혔다면 무시
        if (!_canDamage || _hasDealtDamage) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(Damage);
                Debug.Log("💥 보스 공격 성공! (OnTriggerStay) 피해: " + Damage);

                // 🔥 한 번 피해를 입힌 후 바로 플래그 설정 (중복 피해 방지)
                _hasDealtDamage = true;
            }
        }
    }

}
