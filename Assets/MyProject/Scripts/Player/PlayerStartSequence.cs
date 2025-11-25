using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class PlayerStartSequence : MonoBehaviour
{
    public PlayerMovement Movement;
    public FPSCamera FPSCamera;
    public TextMeshProUGUI CountdownText;
    public GunShoot Gun;   // ★ 추가됨
    private Spawn_enemy _enemySpawner;
    public ColorSequenceManager ColorManager;

    private void Awake()
    {
        // 씬에서 Spawn_enemy 컴포넌트를 찾아 할당
        _enemySpawner = FindFirstObjectByType<Spawn_enemy>();
    }

    private void Start()
    {
        // 움직임 비활성화
        Movement.canMove = false;
        FPSCamera.enabled = false;

        // 총 쏘기 금지
        if (Gun != null)
            Gun.canShootFromStart = false;

        // 플레이어 방향 초기화
        Movement.transform.eulerAngles = new Vector3(0, 180f, 0);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        for (int i = 5; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        CountdownText.text = "GO!";
        ColorManager.SetCubesToWhite();

        _enemySpawner.SpawnAllEnemies();
        yield return new WaitForSeconds(0.5f);

        CountdownText.text = "";

        // 움직임 & 카메라 활성화
        Movement.canMove = true;
        FPSCamera.enabled = true;

        // 총 쏘기 허용
        if (Gun != null)
            Gun.canShootFromStart = true;
    }
}
