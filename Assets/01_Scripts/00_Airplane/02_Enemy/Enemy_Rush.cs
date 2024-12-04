using UnityEngine;

public class Enemy_Rush : Enemy
{
    private bool isOnTarget; // �÷��̾ ���� �������� �ȿ� ���� ��� �÷��̾� �������� ������ ����
    protected Vector3 targetDir; // �÷��̾� ������ �Ҵ���
    [SerializeField] private float rushSpeed; // ���� �ӵ�
    [SerializeField] protected int detectRange; // ���� ���� (���� ������ attackRange)
    private bool isDetect() => Vector3.Distance(transform.position, target.position) < detectRange;
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (isDetect() && !isOnTarget) // �����Ǹ� ����� �ӵ��� ��������
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
    protected override void AirplaneAttack() // ���� ������ Basic�� ���� (�÷��̾�� �������� �� ����)
    {
        EnemyManager.Instance.targetAirplane.TakeDamage(airplaneData.attackDamage);
        Die();
    }
    private void RenewalMovementData() // ���� ������
    {
        targetDir = (transform.position - target.position).normalized;
        transform.up = targetDir;
        movementData.moveDir = -targetDir;
    }
}
