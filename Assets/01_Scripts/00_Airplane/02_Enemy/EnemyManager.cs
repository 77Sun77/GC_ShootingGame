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

    public Action enemyAction;

    public EnemyObjectPool[] enemyObjPools;
    private ObjectPool<Enemy> objPoolTemp;
    public EnemyType enemyTypeTemp;

    private Enemy createTarget;
    private Enemy[] createPrefabsTemp;
    private Enemy enemySpawnTemp;

    public Transform target;
    public AirplaneController targetAirplane;
    
    public Transform[] enemySpawnPoints;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        
    }
    public void Init(Transform target)
    {
        this.target = target;
        targetAirplane = target.GetComponent<AirplaneController>();
        for (int i = 0; i < enemyObjPools.Length; i++)
        {
            for (int j = 1; j <= 4; j++)
                CreateEnemy(enemyObjPools[i], j, 5);
        }
    }
    public Enemy CreateEnemy(EnemyObjectPool enemyObjPool,int rank, int createCount)
    {
        enemyTypeTemp = enemyObjPool.enemyPrefabInfo.enemyType;
        createTarget = enemyObjPool.enemyPrefabInfo.enemies[rank - 1];

        createPrefabsTemp = enemyObjPool.objectPool.CreateObject(createTarget, 5, transform).ToArray();

        foreach(Enemy createPrefab in createPrefabsTemp)
        {
            createPrefab.Init(enemyTypeTemp, rank);
        }

        if (createPrefabsTemp.Length == 1)
            return createPrefabsTemp[0];
        return null;
    }
    public Enemy GetEnemy(EnemyType enemyType, int rank)
    {
        objPoolTemp = enemyObjPools[(int)enemyType].objectPool;
        for (int i=0; i< objPoolTemp.createObj.Count; i++)
        {
            if (objPoolTemp.createObj.Peek().rank == rank)
                break;
            else
            {
                objPoolTemp.DisableObject(objPoolTemp.GetObject());
            }
        }
        return objPoolTemp.GetObject();
    }
    public void DisableEnemy(EnemyType enemyType, Enemy enemy)
    {
        objPoolTemp = enemyObjPools[(int)enemyType].objectPool;
        objPoolTemp.DisableObject(enemy);
    }
    public void SpawnEnemy(EnemyType enemyType, int rank)
    {
        enemySpawnTemp = GetEnemy(enemyType, rank);
        enemySpawnTemp.isDie = false;
        if (enemySpawnTemp == null)
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
    public EnemyType enemyType;
    public Enemy[] enemies;
}

[System.Serializable] 
public class EnemyObjectPool
{
    public EnemyPrefab enemyPrefabInfo;
    public ObjectPool<Enemy> objectPool = new ObjectPool<Enemy>();

}