using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageInfo : ScriptableObject
{
    public List<StageEnemyInfo> stageEnemyInfos = new();
    public float spawnDelay;
    private int maxSpawnCount;
    public int GetMaxSpawnCount()
    {
        if(maxSpawnCount == 0)
        {
            foreach (StageEnemyInfo enemyInfo in stageEnemyInfos)
                maxSpawnCount += enemyInfo.spawnCount;
        }
        return maxSpawnCount;
    }
    public void ResetValue()
    {
        maxSpawnCount = 0;
        foreach (StageEnemyInfo enemyInfo in stageEnemyInfos)
            enemyInfo.spawnCount = enemyInfo.maxCount;
    }
}
[System.Serializable]
public class StageEnemyInfo
{
    public EnemyType enemyType;
    public int maxCount;
    public int spawnCount;
    public int rank = 1;
}
