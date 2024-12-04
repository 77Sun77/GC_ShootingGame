using UnityEngine;

public class Enemy_Shield : Enemy
{
    [SerializeField] private float maxShield; // �� ���� ������
    [SerializeField] private float curShield;
    [SerializeField] private GameObject shieldSprite; // ���� ������ SetActive�� False ��Ŵ
    public override void TakeDamage(float takeAttackDamage) // ���� ������� �� ü�� ��� �� ���� ��� �����ϱ� ���� �������̵� 
    {
        if(curShield > 0)
        {
            curShield -= takeAttackDamage;
            if (curShield < 0)
                shieldSprite.SetActive(false);
        }
        else
            base.TakeDamage(takeAttackDamage);
    }
    protected override void AirplaneAttack() // ���� ������ Basic�� ���� (�÷��̾�� �������� �� ����)
    {
        EnemyManager.Instance.targetAirplane.TakeDamage(airplaneData.attackDamage);
        Die();
    }
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (gameObject.activeInHierarchy && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            AirplaneAttack();
        }
    }
}
