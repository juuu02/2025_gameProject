using UnityEngine;

public enum HitPart
{
    Head,
    Body,
    Arm,
    Leg
}

public class HitBox : MonoBehaviour
{
    public HitPart part;
    public BossHealth boss;   // BossHealth 연결

    public void ApplyDamage(float baseDamage)
    {
        float dmg = baseDamage;

        switch (part)
        {
            case HitPart.Head:
                dmg *= 2f;
                break;

            case HitPart.Body:
                dmg *= 1f;
                break;

            case HitPart.Arm:
                dmg *= 0.7f;
                break;

            case HitPart.Leg:
                dmg *= 0.5f;
                break;
        }

        // 🔥 디버그 출력
        Debug.Log($" Hit: {part} | Damage: {dmg}");

        boss.TakeDamage(dmg);
    }
}
