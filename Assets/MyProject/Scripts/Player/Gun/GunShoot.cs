using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public float FireRate = 10f;
    public float Damage = 20f;

    public Camera FpsCamera;
    public ParticleSystem GunFlash;
    public GameObject HitEffect;

    public Recoil recoil;
    public AmmoSystem ammo;

    public bool canShootFromStart = true;

    public AudioSource audioSource;
    public AudioClip fireSFX;

    float nextTimeToFire = 0f;

    void Update()
    {
        if (!canShootFromStart)
            return;

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
        ammo.ConsumeAmmo();

        if (recoil != null)
            recoil.ApplyRecoil();

        if (GunFlash != null)
            GunFlash.Play();

        if (audioSource != null && fireSFX != null)
            audioSource.PlayOneShot(fireSFX);

        // ⭐ IgnoreBullet 레이어는 제외
        int layerMask = ~LayerMask.GetMask("IgnoreBullet");

        Ray ray = FpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500f, layerMask))
        {
            // ⭐ Collider 없어도 적을 찾도록 변경된 부분
            EnemyHealth enemy = hit.transform.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }

            if (HitEffect != null)
            {
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
