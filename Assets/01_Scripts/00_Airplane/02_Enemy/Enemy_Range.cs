using UnityEngine;

public class Enemy_Range : Enemy
{
    private bool isOnTarget; // 플레이어가 적의 감지범위 안에 들어올 경우 움직임을 멈추고 포탑처럼 총알만 쏨
    public override void Init(EnemyType enemyType, int rank) // 총알을 사용하는 적은 Init을 오버라이드 하여 총알 생성의 과정이 추가됨
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
    protected override void AirplaneAttack() // 직접 공격에서 총알만 발사하도록 바뀐것으로 구조는 기존의 적군 공격과 동일함
    {
        airplaneSkills.BasicShot(this, target.position);
    }

    public override void AirplaneMove() // 플레이어가 감지되었을 때 움직임 통제
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
