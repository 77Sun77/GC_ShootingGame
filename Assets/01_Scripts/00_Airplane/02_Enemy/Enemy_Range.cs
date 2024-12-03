using UnityEngine;

public class Enemy_Range : Enemy
{
    private bool isOnTarget;
    public override void Init(EnemyType enemyType, int rank)
    {
        base.Init(enemyType, rank);
        BulletManager.Instance.CreateEnemyBullet(airplaneData.bulletPrefab, airplaneData.bulletCount, "Player");
    }
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (Vector3.Distance(transform.position, target.position) < attackRange  && !airplaneData.isAttacking)
        {
            if (!isOnTarget)
                isOnTarget = true;
            AirplaneAttack();
        }
    }
    protected override void AirplaneAttack()
    {
        airplaneSkills.BasicShot(this, target.position);
    }

    public override void AirplaneMove()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (movementData.rigid != null && !isOnTarget)
        {
            gameObject.BroadcastMessage("ObjMove", movementData);
        }
        else
        {
            movementData.moveSpeed = 0;
            gameObject.BroadcastMessage("ObjMove", movementData);
        }
    }
}
