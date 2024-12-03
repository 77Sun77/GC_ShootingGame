using UnityEngine;

public class Enemy_Rush : Enemy
{
    private bool isOnTarget;
    protected Vector3 targetDir;
    [SerializeField] private float rushSpeed;
    [SerializeField] protected int detectRange;
    private bool isDetect() => Vector3.Distance(transform.position, target.position) < detectRange;
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (isDetect() && !isOnTarget)
        {
            isOnTarget = true;
            RenewalMovementData();
            movementData.moveSpeed = rushSpeed;
            
        }
        else if (Vector3.Distance(transform.position, target.position) < attackRange)
        {
            AirplaneAttack();
        }
    }
    protected override void AirplaneAttack()
    {
        EnemyManager.Instance.targetAirplane.TakeDamage(airplaneData.attackDamage);
        Die();
    }
    private void RenewalMovementData()
    {
        targetDir = (transform.position - target.position).normalized;
        transform.up = targetDir;
        movementData.moveDir = -targetDir;
    }
}
