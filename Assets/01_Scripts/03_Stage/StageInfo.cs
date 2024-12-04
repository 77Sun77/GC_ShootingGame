using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageInfo : ScriptableObject // ���������� ������ ��� �ִ� (�����ϴ� ���� ����, ���� �ֱ�, �� ���� ����)
{
    public List<StageEnemyInfo> stageEnemyInfos = new(); // �����ϴ� ���� ���� (�ڼ��Ѱ� �ؿ�)
    public float spawnDelay; // ���� �ֱ�
    private int maxSpawnCount; // �� ���� ����
    public int GetMaxSpawnCount() // �� ���� ���ڸ� ����
    {
        if(maxSpawnCount == 0)
        {
            foreach (StageEnemyInfo enemyInfo in stageEnemyInfos)
                maxSpawnCount += enemyInfo.spawnCount;
        }
        return maxSpawnCount;
    }
    public void ResetValue() // ��ũ���ͺ� ������Ʈ�� �����͸� �ʱ� ���·� ����
    {
        maxSpawnCount = 0;
        foreach (StageEnemyInfo enemyInfo in stageEnemyInfos)
            enemyInfo.spawnCount = enemyInfo.maxCount;
    }
}
[System.Serializable]
public class StageEnemyInfo // �����ϴ� ���� ������ ��� Ŭ����
{
    public EnemyType enemyType; // ���� Ÿ��
    public int maxCount; // ���� ����
    public int spawnCount; // ���� ������ ���� (������ ���ڴ� ���� ���ڸ� ���� ���ϰ� ����)
    public int rank = 1; // ���� ��ũ
}
