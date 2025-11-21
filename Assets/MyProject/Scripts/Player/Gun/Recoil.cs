using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float recoilUp = 65f;
    public float recoilBack = 10f;
    public float returnSpeed = 20f;
    public float recoilSpeed = 20f;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    public bool CanShoot { get; private set; } = true;
    // ★ 총이 완전히 복귀해야 true

    void Update()
    {
        // 반동 복귀
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);

        // ★ 복귀 여부 체크
        if (currentRotation.magnitude < 0.1f)
            CanShoot = true;     // 거의 원래 자리 → 다시 쏠 수 있게
        else
            CanShoot = false;    // 반동 중 → 발사 불가
    }

    public void ApplyRecoil()
    {
        // ★ 반동 들어가는 순간은 무조건 발사 불가 상태로 둠
        CanShoot = false;

        targetRotation += new Vector3(-recoilUp, 0f, 0f);
    }
}
