using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager instance;
    public static BulletManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private BulletInfo playerBulletInfo, enemyBulletInfo; // �÷��̾�� ���� �Ѿ˿� ���� �����͸� ������ ���� (�ڼ��Ѱ� �ؿ�)
    [SerializeField] private BulletController playerBulletPrefab; // �÷��̾� �Ѿ� �������� ����
    [SerializeField] private BulletController enemyBulletPrefab; // �� �Ѿ� �������� ����

    private ObjectPool<BulletController> playerBullets = new ObjectPool<BulletController>(); // �÷��̾��� �Ѿ��� �Ҵ��ϴ� ObjectPool
    private ObjectPool<BulletController> enemyBullets = new ObjectPool<BulletController>(); // ���� �Ѿ��� �Ҵ��ϴ� ObjectPool
    private BulletController playerBulletTemp, enemyBulletTemp; // �Ѿ� ���� �� ������ ���� �ӽ÷� �Ҵ�

    private GameObject[] enemyBulletsTemp; // ObjectPool�� ������ �Ѿ˵��� ��� �Ҵ�

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public BulletController GetPlayerBullet() // �÷��̾��� �Ѿ��� ������
    {
        playerBulletTemp = playerBullets.GetObject();
        if (playerBulletTemp == null) // ���� ObjectPool�� ���� ������ �Ѿ��� ���� ��� ���� �� �Ҵ�
        {
            CreatePlayerBullet(playerBulletPrefab, 1, "Enemy");
            playerBulletTemp = playerBullets.GetObject();
        }
        playerBulletTemp.gameObject.SetActive(true); 
        return playerBulletTemp;
    }
    public BulletController GetEnemyBullet() // ������ �Ѿ��� ������
    {
        enemyBulletTemp = enemyBullets.GetObject();
        if (enemyBulletTemp == null) // ���� ObjectPool�� ���� ������ �Ѿ��� ���� ��� ���� �� �Ҵ�
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
            playerBulletPrefab = bulletPrefab; // ������ �Ҵ�

        playerBullets.CreateObject(playerBulletPrefab, maxCount, transform); // ������, ���� ����, �θ� ������Ʈ

        Init_Bullet(playerBullets, targetTag); // ���� ������ ����Ǵ� BulletController�� Init
    }
    public void CreateEnemyBullet(BulletController bulletPrefab, int maxCount, string targetTag)
    {
        if (enemyBulletPrefab == null)
            enemyBulletPrefab = bulletPrefab; // ������ �Ҵ�

        enemyBullets.CreateObject(enemyBulletPrefab, maxCount, transform); // ������, ���� ����, �θ� ������Ʈ

        Init_Bullet(enemyBullets, targetTag);
    }

    private void Init_Bullet(ObjectPool<BulletController> bulletPool, string targetTag)
    {
        foreach (BulletController bullet in bulletPool.createObj) // ������ �Ѿ˵��� Init�� ����
        {
            bullet.Init(targetTag, bulletPool);
        }
    }
}

public struct BulletInfo
{
    public GameObject bulletPrefab; // ������
    public int maxCount; // ���� Ƚ��
}