using UnityEngine;

public class Enemy_Rush : Enemy
{
    private bool isOnTarget; // 플레이어가 적의 감지범위 안에 들어올 경우 플레이어 방향으로 빠르게 돌진
    protected Vector3 targetDir; // 플레이어 방향을 할당함
    [SerializeField] private float rushSpeed; // 돌진 속도
    [SerializeField] protected int detectRange; // 감지 범위 (공격 범위는 attackRange)
    private bool isDetect() => Vector3.Distance(transform.position, target.position) < detectRange;
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (isDetect() && !isOnTarget) // 감지되면 방향과 속도를 재정의함
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
    protected override void AirplaneAttack() // 공격 구조는 Basic과 동일 (플레이어와 근접했을 때 공격)
    {
        EnemyManager.Instance.targetAirplane.TakeDamage(airplaneData.attackDamage);
        Die();
    }
    private void RenewalMovementData() // 방향 재정의
    {
        targetDir = (transform.position - target.position).normalized;
        transform.up = targetDir;
        movementData.moveDir = -targetDir;
    }
}
