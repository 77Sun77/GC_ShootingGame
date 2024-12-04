using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public MovementData movementData; // Movement 클래스를 사용하기 위한 데이터 구조체
    [SerializeField] public float bulletDamage; // 총알의 공격력
    public float disableTime; // 총알은 스폰 후 몇초 이후 비활성화 됨
    public string targetTag; // 적의 총알과 플레이어의 총알을 같은 클래스를 사용하기 위해 타겟 태그로 구분함
    private Airplane hitTarget; // 맞은 타겟의 Airplane을 임시로 캐싱
    private ObjectPool<BulletController> objectPool; // 총알을 사용하는 플레이어와 적의 ObjectPool을 구분하기 위해 Controller에서 할당받음
    public void Init(string targetTag, ObjectPool<BulletController> objectPool)
    {
        this.targetTag = targetTag;
        this.objectPool = objectPool;
    }
    public void ShotBullet(Vector2 pos, Vector2 dir, AirplaneData airplaneData)
    {
        if(movementData.rigid == null)
            movementData.rigid = GetComponent<Rigidbody2D>();

        transform.position = pos; // 위치 초기화

        bulletDamage = airplaneData.attackDamage; // 공격력 초기화
        movementData.moveDir = dir; // 방향 초기화

        BroadcastMessage("ObjMove", movementData); // Rigidbody2D.velocity를 사용한 이동, Movement 컴포넌트의 ObjMove를 브로드캐스팅

        Invoke("DisableGO", disableTime); // 총알은 스폰 후 몇초 이후 비활성화 됨
    }

    public void DisableGO()
    {
        objectPool.DisableObject(this);
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (targetTag == null)
            return;

        if (coll.CompareTag(targetTag)) // 충돌한 오브젝트가 타겟 태그와 동일한지 판단
        {
            hitTarget = coll.GetComponent<Airplane>();

            hitTarget.TakeDamage(bulletDamage);
            DisableGO();

            hitTarget = null;
        }
            
    }
}