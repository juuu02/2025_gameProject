using UnityEngine;

public class ColorSequenceManager : MonoBehaviour
{
    [Header("Cubes")]
    public Renderer Cube1;
    public Renderer Cube2;
    public Renderer Cube3;
    public Renderer Cube4;

    private Renderer[] _cubes;
    private Color[] _baseColors = new Color[4];

    private void Awake()
    {
        _cubes = new Renderer[] { Cube1, Cube2, Cube3, Cube4 };

        // 기본 흰색 설정
        foreach (var cube in _cubes)
            cube.material.color = Color.white;

        // 기본 4색 설정
        _baseColors[0] = Color.red;              // 빨강
        _baseColors[1] = Color.blue;             // 파랑
        _baseColors[2] = Color.yellow;           // 노랑 (보라색 → 변경 완료)
        _baseColors[3] = Color.green;            // 초록 (lightgreen → 초록으로 변경)

    }

    private void Start()
    {
        ShuffleAndApply();
    }

    // ⭐ 라운드마다 호출 가능
    public void ShuffleAndApply()
    {
        Color[] shuffled = (Color[])_baseColors.Clone();

        // Fisher-Yates shuffle
        for (int i = 0; i < shuffled.Length; i++)
        {
            int rnd = Random.Range(i, shuffled.Length);
            (shuffled[i], shuffled[rnd]) = (shuffled[rnd], shuffled[i]);
        }

        for (int i = 0; i < _cubes.Length; i++)
            _cubes[i].material.color = shuffled[i];
    }
}
