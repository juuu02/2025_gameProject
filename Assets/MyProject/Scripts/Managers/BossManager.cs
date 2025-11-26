using UnityEngine;

public class BossManager : MonoBehaviour
{
    private int _sequenceIndex = 0;      // 현재 정답 체크 위치
    private int[] _correctSequenceBoss;      // RoundManager에서 전달된 정답 배열

    private RoundManager _roundManager;

    private void Awake()
    {
        _roundManager = FindFirstObjectByType<RoundManager>();

        if (_roundManager == null)
            Debug.LogError("❌ EnemyManager: RoundManager를 찾을 수 없습니다!");
    }

    // ⭐ RoundManager가 정답 배열을 넣어줌
    public void SetCorrectSequence(int[] sequence)
    {
        _correctSequenceBoss = (int[])sequence.Clone();
        _sequenceIndex = 0;
    }

    // ⭐ Boss Hit 처리
    public void BossHit(Boss bossHit)
    {
        if (_correctSequenceBoss == null)
        {
            Debug.LogError("❌ EnemyManager: 정답 배열이 없습니다!");
            return;
        }

        //int requiredIndex = _correctSequenceBoss[_sequenceIndex]; // 지금 맞춰야 할 색 인덱스
        //int enemyIndex = bossHit.ColorIndex;                 // 이 의 고유 인덱스

        //Debug.Log($"EnemyHit → EnemyIndex: {enemyIndex}, Required: {requiredIndex}");

        //if (enemyIndex == requiredIndex)
        //{
        //    // 정답
        //    enemyHit.DieSuccess();
        //    _sequenceIndex++;

        //    if (_sequenceIndex >= 4)
        //    {
        //        Debug.Log("🎉 모든 정답 순서 완료!");
        //        _roundManager.OnAllEnemiesDefeated();
        //    }
        //}
        //else
        //{
        //    Debug.Log("❌ 오답! 잘못된 Cowboy를 쐈습니다.");
        //}
    }
}
