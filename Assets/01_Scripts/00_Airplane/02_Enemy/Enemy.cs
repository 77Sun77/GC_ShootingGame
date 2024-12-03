using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : Airplane
{
    public EnemyType enemyType;
    public int rank;
    protected AirplaneSkills airplaneSkills = new();

    public Transform target;
    private bool isAttack;
    public bool isDie;
    [SerializeField] protected float attackRange;
    [SerializeField] protected MovementData movementData;
    public abstract void DetectRange();
    public virtual void Init(EnemyType enemyType, int rank)
    {
        this.enemyType = enemyType;
        if(this.rank == 0)
            this.rank = rank;
        target = EnemyManager.Instance.target;
        EnemyManager.Instance.enemyAction += AirplaneMove;
        EnemyManager.Instance.enemyAction += DetectRange;
        EnemyManager.Instance.enemyAction += Respawn;
        
    }

    public override void AirplaneMove()
    {
        if (gameObject.activeInHierarchy && movementData.rigid != null)
        {
            BroadcastMessage("ObjMove", movementData);
        }
    }
    private void Respawn()
    {
        if(transform.position.y < -6f)
        {
            transform.position = EnemyManager.Instance.GetRandomPos();

        }
    }
    protected override void Die()
    {
        if (!isDie)
        {
            EnemyManager.Instance.DisableEnemy(enemyType, this);
            GameManager.Instance.EnemyCountPlus();
            isDie = true;
        }
        
    }
}
