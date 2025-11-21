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

    public bool canShootFromStart = true; // ★ 새로 추가됨

    float nextTimeToFire = 0f;

    void Update()
    {
        if (!canShootFromStart)
            return;   // ★ 카운트다운 중이면 절대 발사 불가

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

        Ray ray = FpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (HitEffect != null)
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
