using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject gameName;      // 첫 화면 텍스트
    public GameObject explainPanel;  // 설명 패널 (처음에는 꺼져있음)

    private int clickCount = 0;      // 클릭 횟수 체크

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            if (clickCount == 1)
            {
                // 1번째 클릭 → GameName 숨기고 설명 등장
                if (gameName != null)
                    gameName.SetActive(false);

                if (explainPanel != null)
                    explainPanel.SetActive(true);
            }
            else if (clickCount == 2)
            {
                // 2번째 클릭 → 다음 씬으로 이동
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
