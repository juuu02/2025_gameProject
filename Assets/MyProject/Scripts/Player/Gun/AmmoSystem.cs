using UnityEngine;
using TMPro;

public class AmmoSystem : MonoBehaviour
{
    public int maxAmmo = 10;
    public int currentAmmo = 10;

    public TMP_Text AmmoText;
    public GunShoot gun;
    private RoundManager _roundManager;

    void Start()
    {
        _roundManager = FindFirstObjectByType<RoundManager>();
        UpdateUI();
    }

    public bool HasAmmo()
    {
        return currentAmmo > 0;
    }

    public void ConsumeAmmo()
    {
        currentAmmo--;
        UpdateUI();

        if (_roundManager != null)
        {
            _roundManager.CheckAmmoAndGameOver(currentAmmo);
        }
    }

    public void UpdateUI()
    {
        if (AmmoText != null)
            AmmoText.text = currentAmmo + "/" + maxAmmo;
    }

    public void ResetAmmo()
    {
        currentAmmo = maxAmmo;
        UpdateUI();
    }
}
