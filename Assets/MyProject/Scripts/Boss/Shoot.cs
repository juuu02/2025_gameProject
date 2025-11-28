using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Camera fpsCam;
    public float damage = 20f;
    public float range = 200f;
    public float fireRate = 1f;   // 1초에 1발
    private float nextTimeToFire = 0f;

    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shooting();
        }
    }

    void Shooting()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            HitBox hb = hit.collider.GetComponent<HitBox>();

            if (hb != null)
                hb.ApplyDamage(damage);

            if (hitEffect != null)
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
