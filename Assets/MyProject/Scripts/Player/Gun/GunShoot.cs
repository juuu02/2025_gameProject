using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public float FireRate = 10f;
    public float Damage = 20f;

    public Camera FpsCamera;
    public ParticleSystem GunFlash;
    public GameObject HitEffect;

    public Recoil recoil;
    public AmmoSystem ammo;       // ★ 추가됨

    float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") &&
            Time.time >= nextTimeToFire &&
            recoil != null &&
            recoil.CanShoot &&
            ammo != null &&
            ammo.HasAmmo())   // ★ 탄약 체크
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // ★ 탄약 소모
        ammo.ConsumeAmmo();

        // ★ 반동 적용
        if (recoil != null)
            recoil.ApplyRecoil();

        // 총구 이펙트
        if (GunFlash != null)
            GunFlash.Play();

        // 레이캐스트
        Ray ray = FpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (HitEffect != null)
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
