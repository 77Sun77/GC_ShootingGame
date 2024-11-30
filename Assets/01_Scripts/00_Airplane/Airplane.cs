using UnityEngine;

public class Airplane : MonoBehaviour
{
    public AirplaneData airplaneData;
    public virtual void TakeDamage(float takeAttackDamage)
    {
        airplaneData.health -= takeAttackDamage;
        if(airplaneData.health <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
    }
    public virtual void AirplaneMove()
    {

    }
    public virtual void AirplaneAttack()
    {

    }
    public virtual void Heal()
    {
        airplaneData.health = airplaneData.maxHealth;
    }
    protected void ResetAttack() => airplaneData.isAttacking = false;
}

[System.Serializable]
public struct AirplaneData
{
    public float maxHealth;
    public float health;
    public float attackDamage;
    public float attackSpeed;
    public bool isActive;
    public Vector2 attackDir;
    public BulletController bulletPrefab;
    public int bulletCount;
    public Transform[] bulletSpawn;
    public bool isAttacking;

}
