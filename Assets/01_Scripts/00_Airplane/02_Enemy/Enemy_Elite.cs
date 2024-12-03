using System.Collections;
using UnityEngine;

public class Enemy_Elite : Enemy
{
    protected Vector3 targetDir;

    [SerializeField] protected int detectRange;
    public override void Init(EnemyType enemyType, int rank)
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

        if (isDetect())
        {
            RenewalMovementData();
        }
        if (gameObject.activeInHierarchy && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            target.GetComponent<Airplane>().TakeDamage(airplaneData.attackDamage);
            Die();
        }
    }
    protected override void AirplaneAttack()
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
