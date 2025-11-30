using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject gameNamePanel;     // GameNamePanel 오브젝트
    public GameObject howToPlayPanel;      // howToPlayPanel 오브젝트

    private int clickCount = 0;

    void Start()
    {
        // 시작 시: 게임 이름만 보이고, 설명 패널은 숨김
        if (gameNamePanel != null)
            gameNamePanel.SetActive(true);

        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            if (clickCount == 1)
            {
                // 첫 번째 클릭 → 이름 패널 OFF, 설명 패널 ON
                if (gameNamePanel != null)
                    gameNamePanel.SetActive(false);

                if (howToPlayPanel != null)
                    howToPlayPanel.SetActive(true);
            }
            else if (clickCount == 2)
            {
                // 두 번째 클릭 → 게임 씬 이동
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
