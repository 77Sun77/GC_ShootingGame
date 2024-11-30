using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager instance;
    public static BulletManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private BulletInfo playerBulletInfo, enemyBulletInfo;
    [SerializeField] private BulletController playerBulletPrefab;
    [SerializeField] private BulletController enemyBulletPrefab;

    private ObjectPool<BulletController> playerBullets = new ObjectPool<BulletController>();
    private ObjectPool<BulletController> enemyBullets = new ObjectPool<BulletController>();
    private BulletController playerBulletTemp, enemyBulletTemp;

    private GameObject[] enemyBulletsTemp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public BulletController GetPlayerBullet()
    {
        playerBulletTemp = playerBullets.GetObject();
        if (playerBulletTemp == null)
        {
            CreatePlayerBullet(playerBulletPrefab, 1, "Enemy");
            playerBulletTemp = playerBullets.GetObject();
        }
        playerBulletTemp.gameObject.SetActive(true);
        return playerBulletTemp;
    }
    public BulletController GetEnemyBullet() 
    {
        enemyBulletTemp = enemyBullets.GetObject();
        if (enemyBulletTemp == null)
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
            playerBulletPrefab = bulletPrefab;

        playerBullets.CreateObject(playerBulletPrefab, maxCount, transform);

        Init_Bullet(playerBullets, targetTag);
    }
    public void CreateEnemyBullet(BulletController bulletPrefab, int maxCount, string targetTag)
    {
        if (enemyBulletPrefab == null)
            enemyBulletPrefab = bulletPrefab;

        enemyBullets.CreateObject(enemyBulletPrefab, maxCount, transform);

        Init_Bullet(enemyBullets, targetTag);
    }

    private void Init_Bullet(ObjectPool<BulletController> bulletPool, string targetTag)
    {
        foreach (BulletController bullet in bulletPool.createObj)
        {
            bullet.Init(targetTag, bulletPool);
        }
    }
}

public struct BulletInfo
{
    public GameObject bulletPrefab;
    public int maxCount;
    public int curBulletCount;
}