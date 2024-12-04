using System.Collections;
using UnityEngine;

public class Enemy_Elite : Enemy
{
    protected Vector3 targetDir;

    [SerializeField] protected int detectRange;
    public override void Init(EnemyType enemyType, int rank) // 총알을 사용하는 적은 Init을 오버라이드 하여 총알 생성의 과정이 추가됨
    {
        base.Init(enemyType, rank);
        BulletManager.Instance.CreateEnemyBullet(airplaneData.bulletPrefab, airplaneData.bulletCount, "Player");
    }
    private void OnEnable()
    {
        Invoke("AirplaneAttack", airplaneData.attackSpeed);
    }
    private bool isDetect() => gameObject.activeInHierarchy && Vector3.Distance(transform.position, target.position) < detectRange;
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (isDetect())  // 플레이어를 지속적으로 따라감 (유도탄 같은 매커니즘)
        {
            RenewalMovementData();
        }
        if (gameObject.activeInHierarchy && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            target.GetComponent<Airplane>().TakeDamage(airplaneData.attackDamage);
            Die();
        }
    }
    protected override void AirplaneAttack() // 몇초당 공격을 반복하는 재귀함수
    {
        if (!gameObject.activeInHierarchy)
            return;

        foreach (Transform bulletSpawn in airplaneData.bulletSpawn)
            airplaneSkills.TripleShot(this, -bulletSpawn.up);
        Invoke("AirplaneAttack", airplaneData.attackSpeed);
    }
    private void RenewalMovementData()
    {
        targetDir = (transform.position - target.position).normalized;
        transform.up = targetDir;
        movementData.moveDir = -targetDir;
    }
}
