using UnityEngine;

public class ColorSequenceManager : MonoBehaviour
{
    [Header("Cubes 연결 (Hierarchy에서 끌어다 넣기)")]
    public Renderer Cube1;
    public Renderer Cube2;
    public Renderer Cube3;
    public Renderer Cube4;

    // 내부 관리용 배열
    private Renderer[] _cubes;

    // 색상 정의 (빨, 노, 초, 파)
    private readonly Color[] ColorByIndex = new Color[4]
    {
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };

    // 정답 인덱스 저장용
    private int[] _shuffledIndices = new int[4];

    private void Awake()
    {
        // 1. 배열에 할당
        _cubes = new Renderer[] { Cube1, Cube2, Cube3, Cube4 };

        // 2. 연결 확인 (연결 안 되어 있으면 에러 띄움)
        for (int i = 0; i < 4; i++)
        {
            if (_cubes[i] == null)
            {
                Debug.LogError($"⛔ [ColorManager] Cube{i + 1}가 연결되지 않았습니다! 인스펙터를 확인하세요.");
            }
        }

        // 3. (중요) 게임 시작 시 기본 정렬(0,1,2,3)로 초기화 해둠
        _shuffledIndices = new int[] { 0, 1, 2, 3 };
    }

    private void Start()
    {
        // ⭐ 시작하자마자 큐브들을 흰색으로 만듭니다. (초기 상태 보장)
        SetCubesToWhite();
    }

    // ⭐ 셔플 및 색상 적용 (카운트다운 시작 전 호출)
    public void ShuffleAndApply()
    {
        // 1. 인덱스 섞기
        int[] indices = new int[] { 0, 1, 2, 3 };
        for (int i = 0; i < 4; i++)
        {
            int rnd = Random.Range(i, 4);
            (indices[i], indices[rnd]) = (indices[rnd], indices[i]);
        }

        _shuffledIndices = indices;

        // 2. 섞인 색상 큐브에 입히기
        RestoreShuffledColors();

        Debug.Log("🎨 [ColorManager] 색상이 섞이고 적용되었습니다!");
    }

    // ⭐ 섞인 정답대로 색상 보여주기
    public void RestoreShuffledColors()
    {
        if (_cubes == null || _cubes.Length == 0) return;

        for (int i = 0; i < _cubes.Length; i++)
        {
            if (_cubes[i] != null)
            {
                int colorIndex = _shuffledIndices[i];
                _cubes[i].material.color = ColorByIndex[colorIndex];
            }
        }
    }

    // ⭐ 큐브를 흰색으로 가리기 (카운트다운 끝난 후 호출)
    public void SetCubesToWhite()
    {
        if (_cubes == null) return;

        for (int i = 0; i < _cubes.Length; i++)
        {
            if (_cubes[i] != null)
                _cubes[i].material.color = Color.white;
        }
    }

    public int[] GetShuffledIndices()
    {
        return (int[])_shuffledIndices.Clone();
    }
}