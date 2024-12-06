using UnityEngine;

public abstract class Airplane : MonoBehaviour
{
    public AirplaneData airplaneData; // 구조체
    public virtual void TakeDamage(float takeAttackDamage) // 공격 받을 때 실행
    {
        airplaneData.health -= takeAttackDamage;
        if(airplaneData.health <= 0)
        {
            Die();
        }
    }
    protected abstract void Die();
    public abstract void AirplaneMove();

    protected abstract void AirplaneAttack();
    public virtual void Heal() // 라운드 시작 및 적군 초기 세팅할 때 사용
    {
        airplaneData.health = airplaneData.maxHealth;
    }
    protected void ResetAttack() => airplaneData.isAttacking = false; // 총알 공속에 맞춰 변수 초기화 (변수가 True일때는 공격 불가)
}

[System.Serializable]
public struct AirplaneData  // 플레이어 또는 적군의 데이터
{
    public float maxHealth;
    public float health;

    public float attackDamage;
    public float attackSpeed;

    public bool isActive;

    public Vector2 attackDir;

    public BulletController bulletPrefab; // 총알 관련 데이터
    public int bulletCount;
    public Transform[] bulletSpawn;

    public bool isAttacking;

}
