using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageInfo : ScriptableObject // 스테이지의 정보를 담고 있다 (등장하는 적의 정보, 스폰 주기, 총 스폰 숫자)
{
    public List<StageEnemyInfo> stageEnemyInfos = new(); // 등장하는 적의 정보 (자세한건 밑에)
    public float spawnDelay; // 스폰 주기
    private int maxSpawnCount; // 총 스폰 숫자
    public int GetMaxSpawnCount() // 총 스폰 숫자를 리턴
    {
        if(maxSpawnCount == 0)
        {
            foreach (StageEnemyInfo enemyInfo in stageEnemyInfos)
                maxSpawnCount += enemyInfo.spawnCount;
        }
        return maxSpawnCount;
    }
    public void ResetValue() // 스크립터블 오브젝트의 데이터를 초기 상태로 리셋
    {
        maxSpawnCount = 0;
        foreach (StageEnemyInfo enemyInfo in stageEnemyInfos)
            enemyInfo.spawnCount = enemyInfo.maxCount;
    }
}
[System.Serializable]
public class StageEnemyInfo // 등장하는 적의 정보를 담는 클래스
{
    public EnemyType enemyType; // 적의 타입
    public int maxCount; // 생성 숫자
    public int spawnCount; // 현재 스폰된 숫자 (스폰된 숫자는 생성 숫자를 넘지 못하게 설계)
    public int rank = 1; // 적의 랭크
}
