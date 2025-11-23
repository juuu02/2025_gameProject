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

    // 🔫 총소리 재생용
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

        // 🔥 여기 추가: 총소리
        if (audioSource != null && fireSFX != null)
            audioSource.PlayOneShot(fireSFX);

        Ray ray = FpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }

            if (HitEffect != null)
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
