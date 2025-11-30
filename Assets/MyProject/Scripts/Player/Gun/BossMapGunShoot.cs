using UnityEngine;

public class BossMapGunShoot : MonoBehaviour
{
    [Header("Gun Settings")]
    public float FireRate = 10f;
    public float Damage = 20f;

    [Header("References")]
    public Camera FpsCamera;
    public ParticleSystem GunFlash;
    public GameObject HitEffect;

    [Header("Systems")]
    public Recoil recoil;
    public AmmoSystem ammo;
    public AudioSource audioSource;

    [Header("State")]
    public bool canShootFromStart = true;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (!canShootFromStart) return;

        if (Input.GetButton("Fire1") &&
            Time.time >= nextTimeToFire &&
            recoil != null &&
            recoil.CanShoot &&
            ammo != null &&
            ammo.HasAmmo())
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (ammo != null)
            ammo.ConsumeAmmo();

        if (recoil != null) recoil.ApplyRecoil();
        if (GunFlash != null) GunFlash.Play();
        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        // IgnoreBullet 레이어만 제외
        int layerMask = ~LayerMask.GetMask("IgnoreBullet");

        Ray ray = FpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 500f, layerMask))
        {
            HitBox bossHitBox = hit.collider.GetComponent<HitBox>();

            if (bossHitBox != null)
            {
                bossHitBox.ApplyDamage(Damage);
            }

            if (HitEffect != null)
            {
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        else
        {
            // 🔥 Raycast가 아무것도 맞추지 못했을 때 (피격 실패)
            Debug.Log("Raycast Missed everything.");
        }
    }
}
