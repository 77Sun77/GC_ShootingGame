using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : Airplane
{
    public EnemyType enemyType; // ���� Ÿ���� 5������ �з�
    public int rank; // ���� ��ũ�� 4������ �з��Ͽ� �� Ÿ�Դ� 4���� ��ũ�� ������ (�� 20���� ����)
    protected AirplaneSkills airplaneSkills = new(); 

    public Transform target; // �÷��̾ �Ҵ��Ѵ�
    public bool isDie;

    [SerializeField] protected float attackRange; // ���� ���� (�÷��̾ �ν��ؾ� �ϴ� ���� ����)
    [SerializeField] protected MovementData movementData; // Movement Ŭ������ ����ϱ� ���� ������ ����ü
    public abstract void DetectRange(); // �÷��̾���� �Ÿ��� �Ǻ��ϴ� �Լ�
    public virtual void Init(EnemyType enemyType, int rank) // ���� ���� �� ������ �����ϰ� �̺�Ʈ�� �Լ��� ������ (Update���� �����ؾ� �ϴ� �Լ��� EnemyManager���� �ѹ��� �����)
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
    private void Respawn() // ���� ���� �ʰ� �÷��̾�� ������ �������� �� �ٽ� ���� �÷���
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
            EnemyManager.Instance.DisableEnemy(enemyType, this); // ObjectPool�� ���� ���� ���� ��ȯ�ϱ� ���� ���
            GameManager.Instance.EnemyCountPlus(); // �������� Ŭ���� ���θ� Ȯ���ϱ� ���� ������ ī��Ʈ�� �÷���
            isDie = true;
        }
        
    }
}
