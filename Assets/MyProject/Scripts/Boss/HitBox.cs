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
    public BossHealth boss;

    private void Start()
    {
        if (boss == null)
            boss = GetComponentInParent<BossHealth>();
    }

    public void ApplyDamage(float baseDamage)
    {
        float dmg = baseDamage;

        switch (part)
        {
            case HitPart.Head: dmg *= 2f; break;
            case HitPart.Body: dmg *= 1f; break;
            case HitPart.Arm: dmg *= 0.7f; break;
            case HitPart.Leg: dmg *= 0.5f; break;
        }

        boss.TakeDamage(dmg);

        Debug.Log($"Hit: {part} | Damage: {dmg} | Boss HP: {boss.HP}");
    }
}
