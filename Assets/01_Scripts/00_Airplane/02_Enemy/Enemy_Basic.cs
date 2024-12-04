using UnityEngine;

public class Enemy_Basic : Enemy
{
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (gameObject.activeInHierarchy && Vector3.Distance(transform.position, target.position) < attackRange) // 플레이어와 근접했을 때 공격
        {
            AirplaneAttack();
        }
    }
    protected override void AirplaneAttack()
    {
        EnemyManager.Instance.targetAirplane.TakeDamage(airplaneData.attackDamage);
        Die();
    }

}
