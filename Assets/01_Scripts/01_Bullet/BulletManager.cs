using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager instance;
    public static BulletManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private BulletInfo playerBulletInfo, enemyBulletInfo; // 플레이어와 적의 총알에 대한 데이터를 가지고 있음 (자세한건 밑에)
    [SerializeField] private BulletController playerBulletPrefab; // 플레이어 총알 프리팹을 저장
    [SerializeField] private BulletController enemyBulletPrefab; // 적 총알 프리팹을 저장

    private ObjectPool<BulletController> playerBullets = new ObjectPool<BulletController>(); // 플레이어의 총알을 할당하는 ObjectPool
    private ObjectPool<BulletController> enemyBullets = new ObjectPool<BulletController>(); // 적의 총알을 할당하는 ObjectPool
    private BulletController playerBulletTemp, enemyBulletTemp; // 총알 스폰 시 스폰할 적을 임시로 할당

    private GameObject[] enemyBulletsTemp; // ObjectPool시 생성된 총알들을 잠시 할당

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public BulletController GetPlayerBullet() // 플레이어의 총알을 리턴함
    {
        playerBulletTemp = playerBullets.GetObject();
        if (playerBulletTemp == null) // 만약 ObjectPool에 스폰 가능한 총알이 없을 경우 생성 후 할당
        {
            CreatePlayerBullet(playerBulletPrefab, 1, "Enemy");
            playerBulletTemp = playerBullets.GetObject();
        }
        playerBulletTemp.gameObject.SetActive(true); 
        return playerBulletTemp;
    }
    public BulletController GetEnemyBullet() // 적의의 총알을 리턴함
    {
        enemyBulletTemp = enemyBullets.GetObject();
        if (enemyBulletTemp == null) // 만약 ObjectPool에 스폰 가능한 총알이 없을 경우 생성 후 할당
        {
            CreateEnemyBullet(enemyBulletPrefab, 1, "Player");
            enemyBulletTemp = enemyBullets.GetObject();
        }
        enemyBulletTemp.gameObject.SetActive(true);
        return enemyBulletTemp;
    }
    public void CreatePlayerBullet(BulletController bulletPrefab, int maxCount, string targetTag)
    {
        if (playerBulletPrefab == null)
            playerBulletPrefab = bulletPrefab; // 프리팹 할당

        playerBullets.CreateObject(playerBulletPrefab, maxCount, transform); // 프리팹, 생성 숫자, 부모 오브젝트

        Init_Bullet(playerBullets, targetTag); // 최초 생성시 실행되는 BulletController의 Init
    }
    public void CreateEnemyBullet(BulletController bulletPrefab, int maxCount, string targetTag)
    {
        if (enemyBulletPrefab == null)
            enemyBulletPrefab = bulletPrefab; // 프리팹 할당

        enemyBullets.CreateObject(enemyBulletPrefab, maxCount, transform); // 프리팹, 생성 숫자, 부모 오브젝트

        Init_Bullet(enemyBullets, targetTag);
    }

    private void Init_Bullet(ObjectPool<BulletController> bulletPool, string targetTag)
    {
        foreach (BulletController bullet in bulletPool.createObj) // 생성된 총알들의 Init를 실행
        {
            bullet.Init(targetTag, bulletPool);
        }
    }
}

public struct BulletInfo
{
    public GameObject bulletPrefab; // 프리팹
    public int maxCount; // 생성 횟수
}