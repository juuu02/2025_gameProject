using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerStartSequenceBoss : MonoBehaviour
{
    [Header("Player References")]
    public PlayerMovement Movement;
    public FPSCamera FPSCamera;
    public GunShoot Gun;

    [Header("UI")]
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI RoundText; // 🔥 RoundText 연결할 변수 추가!

    [Header("Settings")]
    public float RoundTextDuration = 2.0f; // RoundText가 떠있는 시간

    [Header("Start Point")]
    public Transform PlayerStartPoint;

    [Header("Boss Control")]
    public Boss_control BossControl;

    private void Start()
    {
        // 1. 게임 시작하자마자 플레이어 얼리기
        Movement.canMove = false;
        FPSCamera.enabled = false;
        if (Gun != null) Gun.canShootFromStart = false;

        if (BossControl != null)
        {
            BossControl.canMove = false;
            Debug.Log("🔒 보스 움직임 잠금.");
        }

        // 🔥CountdownText만 끄고, RoundText는 켜둡니다!
        if (CountdownText != null) CountdownText.gameObject.SetActive(false);

        if (RoundText != null)
        {
            RoundText.gameObject.SetActive(true); // 시작하자마자 보이게 켬
            RoundText.text = "BOSS ROUND";
        }

        // 3. 전체 시퀀스 시작
        StartCoroutine(StartBossSequence());
    }

    private IEnumerator StartBossSequence()
    {
        // --- [단계 1] 플레이어 위치 리셋 ---
        ResetPlayerPosition();

        // --- [단계 2] Round Text 대기 ---
        // 이미 Start에서 켜뒀으니, 여기선 2초 기다리기만 하면 됨
        yield return new WaitForSeconds(RoundTextDuration);

        // 🔥 2초 뒤에 RoundText 끄기
        if (RoundText != null)
            RoundText.gameObject.SetActive(false);

        // --- [단계 3] Countdown Text 시작 (3, 2, 1) ---
        // 🔥 바로 이어서 카운트다운 켜기
        if (CountdownText != null)
        {
            CountdownText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                CountdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            // GO! 표시
            CountdownText.text = "GO!";
            yield return new WaitForSeconds(0.5f);

            CountdownText.text = "";
            CountdownText.gameObject.SetActive(false);
        }

        // --- [단계 4] 게임 시작! (조작 잠금 해제) ---
        Movement.canMove = true;
        FPSCamera.enabled = true;
        if (Gun != null) Gun.canShootFromStart = true;

        // 🔥 카운트다운이 끝났으므로 보스 움직임 활성화
        if (BossControl != null)
        {
            BossControl.canMove = true;
            Debug.Log("🔓 보스 움직임 활성화 완료.");
        }

        Debug.Log("🚀 보스전 시작! 플레이어 조작 활성화 완료.");
    }
    private void ResetPlayerPosition()
    {
        CharacterController cc = Movement.GetComponent<CharacterController>();

        // 이동을 위해 잠시 CC 끄기
        if (cc != null) cc.enabled = false;

        // 위치 및 회전 강제 설정
        Movement.transform.position = PlayerStartPoint.position;
        // Movement.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 보스를 바라보게 설정

        // 물리력 초기화 (가속도 제거)
        Rigidbody rb = Movement.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // 구버전이면 velocity
            rb.angularVelocity = Vector3.zero;
        }

        // 위치 이동 후 CC 다시 켜기 (물리 충돌을 위해)
        // 주의: 여기서 켜도 Movement.canMove가 false라 움직이지는 못함! (의도된 동작)
        if (cc != null) cc.enabled = true;
    }
}