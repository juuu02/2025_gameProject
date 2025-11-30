using UnityEngine;

public class Spawn_Boss : MonoBehaviour
{
    [Header("Hierarchy Boss")]
    public GameObject ExistingBoss; // 🔥 하이어라키에 있는 보스를 여기에 드래그하세요!

    public Vector3 startPos = new Vector3(1.74f, 5.13f, -5.389f); // 보스 시작 위치

    [Header("Boss Move Settings")]
    public GameObject Player;

    private BossManager manager;

    private void Awake()
    {
        // BossManager 찾기 (최신 버전 호환)
        manager = FindFirstObjectByType<BossManager>();

        if (manager == null)
            Debug.LogError("❌ Spawn_Boss: BossManager가 씬에 없습니다!");
    }

    // BossManager에서 Start() 때 호출됨
    public void SpawnBoss()
    {
        if (ExistingBoss == null)
        {
            Debug.LogError("❌ Spawn_Boss: 'ExistingBoss'가 비어있습니다! 인스펙터에서 보스를 연결해주세요.");
            return;
        }

        // 1. 보스 활성화 (혹시 꺼져 있을까봐)
        ExistingBoss.SetActive(true);

        // 🔥 보스 순간이동! (복제가 아님)
        ExistingBoss.transform.position = startPos;
        ExistingBoss.transform.rotation = Quaternion.Euler(0, 180, 0);

        // 3. 목표 지점(Target) 설정

        Boss_control control = ExistingBoss.GetComponent<Boss_control>();
        if (control != null)
        {
            // control.SetDestinationTarget(targetVector); // 기존 함수 호출 대신

            // 🔥 수정된 SetTarget 함수 호출
            if (Player != null)
            {
                control.SetTarget(Player.transform);
            }
            else
            {
                Debug.LogError("❌ Spawn_Boss: 'Player' 오브젝트가 연결되지 않았습니다!");
            }
        }
        // ...

        // 4. BossManager에 등록
        BossHealth bh = ExistingBoss.GetComponent<BossHealth>();
        if (bh != null)
        {
            if (manager != null) manager.RegisterBoss(bh);
        }
        else
        {
            Debug.LogError("❌ Boss 오브젝트에 BossHealth 스크립트가 없습니다!");
        }

        Debug.Log($"🚀 Scene 보스 배치 완료! 시작 위치: {startPos}");
    }
}