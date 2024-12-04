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

    public Action enemyAction; // 적들의 함수를 구독하여 EnemyManager Update에서 실행

    public EnemyObjectPool[] enemyObjPools; // 적군의 종류를 나눠서 데이터를 분리한 클래스 (자세한건 밑으로)
    private ObjectPool<Enemy> objPoolTemp;
    public EnemyType enemyTypeTemp; 

    private Enemy createTarget; // ObjectPool시 적의 데이터를 잠시 할당
    private Enemy[] createPrefabsTemp; // ObjectPool시 생성된 적들을 잠시 할당

    private Enemy enemySpawnTemp; // 적 스폰 시 스폰할 적을 임시로 할당

    public Transform target; // 플레이어
    public AirplaneController targetAirplane; // 플레이어 클래스를 캐싱
    
    public Transform[] enemySpawnPoints; // 적의 스폰포인트 (현 3개)

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        
    }
    public void Init(Transform target) // 플레이어 기체를 선택했을 시 실행함 (실행함과 동시에 게임 시작)
    {
        this.target = target;
        targetAirplane = target.GetComponent<AirplaneController>();

        for (int i = 0; i < enemyObjPools.Length; i++) // 적군 각 타입, 랭크 당 5개씩 생성
        {
            for (int j = 1; j <= 4; j++)
                CreateEnemy(enemyObjPools[i], j, 5);
        }
    }
    public Enemy CreateEnemy(EnemyObjectPool enemyObjPool,int rank, int createCount)
    {
        enemyTypeTemp = enemyObjPool.enemyPrefabInfo.enemyType; // 타입 임시 저장
        createTarget = enemyObjPool.enemyPrefabInfo.enemies[rank - 1]; // 타입의 대한 랭크 임시 저장

        createPrefabsTemp = enemyObjPool.objectPool.CreateObject(createTarget, 5, transform).ToArray(); // ObjectPool로 생성된 적들의 데이터를 임시 저장 (프리팹, 생성 숫자, 부모 오브젝트를 넘겨줌)

        foreach (Enemy createPrefab in createPrefabsTemp) // 최초 생성시 실행되는 Enemy의 Init
        {
            createPrefab.Init(enemyTypeTemp, rank);
        }

        if (createPrefabsTemp.Length == 1)
            return createPrefabsTemp[0];
        return null;
    }
    public Enemy GetEnemy(EnemyType enemyType, int rank) // 적을 스폰시킬때 타입과 랭크를 받아 해당하는 적을 리턴함
    {
        objPoolTemp = enemyObjPools[(int)enemyType].objectPool; // 특정 타입에 대한 ObjectPool을 임시 저장
        for (int i=0; i< objPoolTemp.createObj.Count; i++) // 타입 ObjectPool에 rank가 동일한 적이 있는지 판단
        {
            if (objPoolTemp.createObj.Peek().rank == rank)
                break;
            else
            {
                objPoolTemp.DisableObject(objPoolTemp.GetObject());
            }
        }
        return objPoolTemp.GetObject(); // rank가 동일한 적이 있다면 그 적을 리턴하고 없으면 null을 리턴함
    }
    public void DisableEnemy(EnemyType enemyType, Enemy enemy) // 적이 죽었을 때 오브젝트 풀에 적을 넘겨줌
    {
        objPoolTemp = enemyObjPools[(int)enemyType].objectPool;
        objPoolTemp.DisableObject(enemy);
    }
    public void SpawnEnemy(EnemyType enemyType, int rank) // 특정 적을 스폰할때 실행
    {
        enemySpawnTemp = GetEnemy(enemyType, rank); // 위에 GetEnemy를 호출
        enemySpawnTemp.isDie = false; // 죽어있는 경우가 있기 때문에 초기화
        if (enemySpawnTemp == null) // 만약 ObjectPool에 스폰 가능한 적이 없어 null이 리턴된 경우 생성과 동시에 할당
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
    public EnemyType enemyType; // 적의 5가지 타입
    public Enemy[] enemies; // 각 랭크가 나뉘어진 프리팹을 할당함
}

[System.Serializable] 
public class EnemyObjectPool
{
    public EnemyPrefab enemyPrefabInfo; // 적의 프리팹 정보가 들어있는 구조체
    public ObjectPool<Enemy> objectPool = new ObjectPool<Enemy>(); // 적의 타입마다 ObjectPool이 존재하여 특정 타입을 지정할 수 있도록 한다.

}