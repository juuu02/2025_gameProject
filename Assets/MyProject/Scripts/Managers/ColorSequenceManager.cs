using UnityEngine;

public class ColorSequenceManager : MonoBehaviour
{
    [Header("Cubes (왼 → 오른쪽 순서)")]
    public Renderer Cube1;
    public Renderer Cube2;
    public Renderer Cube3;
    public Renderer Cube4;

    private Renderer[] _cubes;

    // 🔥 Ju 색상 인덱스 규칙
    // 0=Red, 1=Yellow, 2=Green, 3=Blue
    private readonly Color[] ColorByIndex = new Color[4]
    {
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };

    // 🔥 Cube에 들어가는 정답 인덱스 배열
    private int[] _shuffledIndices = new int[4];

    private void Awake()
    {
        _cubes = new Renderer[] { Cube1, Cube2, Cube3, Cube4 };

        for (int i = 0; i < 4; i++)
        {
            if (_cubes[i] == null)
                Debug.LogError($"❌ Cube{i + 1}가 연결되지 않았습니다!");
        }
    }

    private void Start()
    {
        ShuffleAndApply();
    }

    // ⭐ 매 라운드마다 Cube 인덱스를 셔플
    public void ShuffleAndApply()
    {
        // 0~3 인덱스 기반 셔플
        int[] indices = new int[] { 0, 1, 2, 3 };

        for (int i = 0; i < 4; i++)
        {
            int rnd = Random.Range(i, 4);
            (indices[i], indices[rnd]) = (indices[rnd], indices[i]);
        }

        _shuffledIndices = indices;

        // Cube에 인덱스별 색 지정
        for (int i = 0; i < 4; i++)
        {
            int idx = _shuffledIndices[i];
            _cubes[i].material.color = ColorByIndex[idx];
        }

        Debug.Log($"[ColorSequenceManager] 정답 인덱스: {_shuffledIndices[0]}, {_shuffledIndices[1]}, {_shuffledIndices[2]}, {_shuffledIndices[3]}");
    }

    // 정답 인덱스 가져오기
    public int[] GetShuffledIndices()
    {
        return (int[])_shuffledIndices.Clone();
    }

    // 특정 Cube의 인덱스 가져오기
    public int GetIndexAt(int i)
    {
        return _shuffledIndices[i];
    }

    // 🔥 5초 카운트다운 종료 → 큐브를 흰색으로
    public void SetCubesToWhite()
    {
        for (int i = 0; i < _cubes.Length; i++)
            _cubes[i].material.color = Color.white;
    }

    // 🔥 라운드 시작 → 셔플된 색상대로 복구
    public void RestoreShuffledColors()
    {
        for (int i = 0; i < _cubes.Length; i++)
        {
            int idx = _shuffledIndices[i];
            _cubes[i].material.color = ColorByIndex[idx];
        }
    }

}
