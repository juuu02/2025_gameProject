using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class PlayerStartSequence : MonoBehaviour
{
    public PlayerMovement Movement;
    public FPSCamera FPSCamera;
    public TextMeshProUGUI CountdownText, RoundText;
    public GunShoot Gun;   // ★ 추가됨
    private Spawn_enemy _enemySpawner;
    public ColorSequenceManager ColorManager;
    public RoundManager RoundManager;

    private void Awake()
    {
        // 씬에서 Spawn_enemy 컴포넌트를 찾아 할당
        _enemySpawner = FindFirstObjectByType<Spawn_enemy>();
    }

    private void Start()
    {
        // 초기화만 하고, StartCountdown은 절대 실행하지 않는다
        Movement.canMove = false;
        FPSCamera.enabled = false;

        if (Gun != null)
            Gun.canShootFromStart = false;

    }


    private IEnumerator StartCountdown()
    {
        // 1. 카운트다운 텍스트 끄고 시작
        CountdownText.gameObject.SetActive(false);
        CountdownText.text = "";

        // 2. "Round X" 텍스트 1.5초 보여주기
        RoundText.text = $"Round {RoundManager.CurrentRound}";
        RoundText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        RoundText.gameObject.SetActive(false);

        // 🔥 [핵심 변경] 카운트다운 시작과 동시에 '색상'을 보여줍니다 (암기 시간)
        // 이때 내부적으로 셔플이 일어나고 색이 칠해집니다.
        ColorManager.ShuffleAndApply();

        // 3. 카운트다운 3초 진행 (플레이어는 이때 색을 외웁니다)
        CountdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // 4. "GO!" 출력
        CountdownText.text = "GO!";

        // 🔥 [핵심 변경] 게임 시작 직전, 큐브를 다시 '흰색'으로 가립니다 (문제 감추기)
        ColorManager.SetCubesToWhite();

        // 5. 적 소환 및 대기
        _enemySpawner.SpawnAllEnemies();
        yield return new WaitForSeconds(0.5f);

        CountdownText.text = "";

        // 6. 플레이어 조작 해제
        Movement.canMove = true;
        FPSCamera.enabled = true;
        if (Gun != null) Gun.canShootFromStart = true;

        // 7. 라운드 매니저에게 "실제 게임 시작" 알림
        RoundManager.StartRound();
    }

    public void PlayStartSequence()
    {
        // 🔥 CharacterController 잠깐 꺼야 위치 리셋이 정확하게 먹힘
        CharacterController cc = Movement.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 위치 / 회전 리셋
        Movement.transform.position = RoundManager.PlayerStartPoint.position;
        Movement.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        // Rigidbody velocity 초기화
        Rigidbody rb = Movement.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 조작 잠금
        Movement.canMove = false;
        FPSCamera.enabled = false;
        if (Gun != null)
            Gun.canShootFromStart = false;

        // 🔥 위치/회전 리셋 끝났으니 CharacterController 다시 켜기
        if (cc != null) cc.enabled = true;

        // 카운트다운 실행
        StartCoroutine(StartCountdown());
    }


    public void ResetWeaponAmmo()
    {
        AmmoSystem ammo = GetComponentInChildren<AmmoSystem>();
        if (ammo != null)
            ammo.ResetAmmo();
    }


}
