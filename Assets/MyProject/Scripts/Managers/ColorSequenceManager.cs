using UnityEngine;

public class ColorSequenceManager : MonoBehaviour
{
    [Header("Cubes (왼 → 오른쪽 순서)")]
    public Renderer Cube1;
    public Renderer Cube2;
    public Renderer Cube3;
    public Renderer Cube4;

    private Renderer[] _cubes;
    private readonly Color[] _baseColors = new Color[4];
    private Color[] _shuffledColors = new Color[4];

    private void Awake()
    {
        // 4개 큐브 자동 배열
        _cubes = new Renderer[] { Cube1, Cube2, Cube3, Cube4 };

        // null 체크
        for (int i = 0; i < _cubes.Length; i++)
        {
            if (_cubes[i] == null)
                Debug.LogError($"❌ ColorSequenceManager: Cube{i + 1}가 연결되지 않았습니다!");
        }

        // 기본 큐브 색상 흰색 초기화
        foreach (var cube in _cubes)
            cube.material.color = Color.white;

        // 기본 4색 설정 (Ju가 말한 순서대로)
        _baseColors[0] = Color.red;      // 빨강
        _baseColors[1] = Color.blue;     // 파랑
        _baseColors[2] = Color.yellow;   // 노랑
        _baseColors[3] = Color.green;    // 초록
    }

    private void Start()
    {
        ShuffleAndApply();
    }

    // ⭐ 매 라운드마다 호출
    public void ShuffleAndApply()
    {
        // _baseColors 복사
        Color[] shuffled = (Color[])_baseColors.Clone();

        // Fisher-Yates Shuffle
        for (int i = 0; i < shuffled.Length; i++)
        {
            int rnd = Random.Range(i, shuffled.Length);
            (shuffled[i], shuffled[rnd]) = (shuffled[rnd], shuffled[i]);
        }

        _shuffledColors = shuffled;

        // 큐브에 색 적용
        for (int i = 0; i < _cubes.Length; i++)
            _cubes[i].material.color = _shuffledColors[i];

        // 디버그용
        Debug.Log($"Shuffled Colors: {_shuffledColors[0]}, {_shuffledColors[1]}, {_shuffledColors[2]}, {_shuffledColors[3]}");
    }

    // 특정 인덱스 색상 가져오기
    public Color GetColorAtIndex(int index)
    {
        if (index < 0 || index >= _shuffledColors.Length)
        {
            Debug.LogError("❌ ColorSequenceManager: 잘못된 인덱스 접근!");
            return Color.clear;
        }

        return _shuffledColors[index];
    }

    // 전체 색 배열 가져오기 (EnemyManager가 사용)
    public Color[] GetShuffledColors()
    {
        return (Color[])_shuffledColors.Clone();  // 외부 수정 방지
    }
}
