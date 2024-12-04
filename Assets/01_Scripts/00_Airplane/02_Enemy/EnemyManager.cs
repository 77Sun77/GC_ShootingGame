using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get { return instance; }
    }

    public Action enemyAction; // ������ �Լ��� �����Ͽ� EnemyManager Update���� ����

    public EnemyObjectPool[] enemyObjPools; // ������ ������ ������ �����͸� �и��� Ŭ���� (�ڼ��Ѱ� ������)
    private ObjectPool<Enemy> objPoolTemp;
    public EnemyType enemyTypeTemp; 

    private Enemy createTarget; // ObjectPool�� ���� �����͸� ��� �Ҵ�
    private Enemy[] createPrefabsTemp; // ObjectPool�� ������ ������ ��� �Ҵ�

    private Enemy enemySpawnTemp; // �� ���� �� ������ ���� �ӽ÷� �Ҵ�

    public Transform target; // �÷��̾�
    public AirplaneController targetAirplane; // �÷��̾� Ŭ������ ĳ��
    
    public Transform[] enemySpawnPoints; // ���� ��������Ʈ (�� 3��)

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        
    }
    public void Init(Transform target) // �÷��̾� ��ü�� �������� �� ������ (�����԰� ���ÿ� ���� ����)
    {
        this.target = target;
        targetAirplane = target.GetComponent<AirplaneController>();

        for (int i = 0; i < enemyObjPools.Length; i++) // ���� �� Ÿ��, ��ũ �� 5���� ����
        {
            for (int j = 1; j <= 4; j++)
                CreateEnemy(enemyObjPools[i], j, 5);
        }
    }
    public Enemy CreateEnemy(EnemyObjectPool enemyObjPool,int rank, int createCount)
    {
        enemyTypeTemp = enemyObjPool.enemyPrefabInfo.enemyType; // Ÿ�� �ӽ� ����
        createTarget = enemyObjPool.enemyPrefabInfo.enemies[rank - 1]; // Ÿ���� ���� ��ũ �ӽ� ����

        createPrefabsTemp = enemyObjPool.objectPool.CreateObject(createTarget, 5, transform).ToArray(); // ObjectPool�� ������ ������ �����͸� �ӽ� ���� (������, ���� ����, �θ� ������Ʈ�� �Ѱ���)

        foreach (Enemy createPrefab in createPrefabsTemp) // ���� ������ ����Ǵ� Enemy�� Init
        {
            createPrefab.Init(enemyTypeTemp, rank);
        }

        if (createPrefabsTemp.Length == 1)
            return createPrefabsTemp[0];
        return null;
    }
    public Enemy GetEnemy(EnemyType enemyType, int rank) // ���� ������ų�� Ÿ�԰� ��ũ�� �޾� �ش��ϴ� ���� ������
    {
        objPoolTemp = enemyObjPools[(int)enemyType].objectPool; // Ư�� Ÿ�Կ� ���� ObjectPool�� �ӽ� ����
        for (int i=0; i< objPoolTemp.createObj.Count; i++) // Ÿ�� ObjectPool�� rank�� ������ ���� �ִ��� �Ǵ�
        {
            if (objPoolTemp.createObj.Peek().rank == rank)
                break;
            else
            {
                objPoolTemp.DisableObject(objPoolTemp.GetObject());
            }
        }
        return objPoolTemp.GetObject(); // rank�� ������ ���� �ִٸ� �� ���� �����ϰ� ������ null�� ������
    }
    public void DisableEnemy(EnemyType enemyType, Enemy enemy) // ���� �׾��� �� ������Ʈ Ǯ�� ���� �Ѱ���
    {
        objPoolTemp = enemyObjPools[(int)enemyType].objectPool;
        objPoolTemp.DisableObject(enemy);
    }
    public void SpawnEnemy(EnemyType enemyType, int rank) // Ư�� ���� �����Ҷ� ����
    {
        enemySpawnTemp = GetEnemy(enemyType, rank); // ���� GetEnemy�� ȣ��
        enemySpawnTemp.isDie = false; // �׾��ִ� ��찡 �ֱ� ������ �ʱ�ȭ
        if (enemySpawnTemp == null) // ���� ObjectPool�� ���� ������ ���� ���� null�� ���ϵ� ��� ������ ���ÿ� �Ҵ�
            enemySpawnTemp = CreateEnemy(enemyObjPools[(int)enemyType], rank, 1);

        enemySpawnTemp.gameObject.SetActive(true);
        enemySpawnTemp.transform.position = GetRandomPos();
        enemySpawnTemp.Heal();
    }
    public Vector3 GetRandomPos()
    {
        return enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)].position;
    }
    private void Update()
    {
        enemyAction?.Invoke();
    }
}
public enum EnemyType {
    Basic=0, 
    Range=1, 
    Rush=2,
    Shield=3,
    Elite=4
};

[System.Serializable]
public struct EnemyPrefab
{
    public EnemyType enemyType; // ���� 5���� Ÿ��
    public Enemy[] enemies; // �� ��ũ�� �������� �������� �Ҵ���
}

[System.Serializable] 
public class EnemyObjectPool
{
    public EnemyPrefab enemyPrefabInfo; // ���� ������ ������ ����ִ� ����ü
    public ObjectPool<Enemy> objectPool = new ObjectPool<Enemy>(); // ���� Ÿ�Ը��� ObjectPool�� �����Ͽ� Ư�� Ÿ���� ������ �� �ֵ��� �Ѵ�.

}