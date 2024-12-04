using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : Airplane
{
    public EnemyType enemyType; // 적군 타입을 5가지로 분류
    public int rank; // 적군 랭크를 4가지로 분류하여 한 타입당 4개의 랭크를 가진다 (총 20개의 적군)
    protected AirplaneSkills airplaneSkills = new(); 

    public Transform target; // 플레이어를 할당한다
    public bool isDie;

    [SerializeField] protected float attackRange; // 공격 범위 (플레이어를 인식해야 하는 적만 존재)
    [SerializeField] protected MovementData movementData; // Movement 클래스를 사용하기 위한 데이터 구조체
    public abstract void DetectRange(); // 플레이어와의 거리를 판별하는 함수
    public virtual void Init(EnemyType enemyType, int rank) // 최초 생성 때 변수를 지정하고 이벤트에 함수를 구독함 (Update에서 구동해야 하는 함수는 EnemyManager에서 한번에 실행됨)
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
    private void Respawn() // 적이 죽지 않고 플레이어보다 밑으로 내려갔을 때 다시 위로 올려줌
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
            EnemyManager.Instance.DisableEnemy(enemyType, this); // ObjectPool에 죽은 적의 값을 반환하기 위해 사용
            GameManager.Instance.EnemyCountPlus(); // 스테이지 클리어 여부를 확인하기 위해 죽을때 카운트를 올려줌
            isDie = true;
        }
        
    }
}
