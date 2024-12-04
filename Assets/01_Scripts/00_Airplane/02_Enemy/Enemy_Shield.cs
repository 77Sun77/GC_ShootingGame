using UnityEngine;

public class Enemy_Shield : Enemy
{
    [SerializeField] private float maxShield; // 방어막 관련 데이터
    [SerializeField] private float curShield;
    [SerializeField] private GameObject shieldSprite; // 방어막이 깨지면 SetActive를 False 시킴
    public override void TakeDamage(float takeAttackDamage) // 방어막이 살아있을 때 체력 대신 방어막 먼저 까도록 구현하기 위해 오버라이드 
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
    protected override void AirplaneAttack() // 공격 구조는 Basic과 동일 (플레이어와 근접했을 때 공격)
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
