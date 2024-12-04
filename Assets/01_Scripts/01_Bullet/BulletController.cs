using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public MovementData movementData; // Movement Ŭ������ ����ϱ� ���� ������ ����ü
    [SerializeField] public float bulletDamage; // �Ѿ��� ���ݷ�
    public float disableTime; // �Ѿ��� ���� �� ���� ���� ��Ȱ��ȭ ��
    public string targetTag; // ���� �Ѿ˰� �÷��̾��� �Ѿ��� ���� Ŭ������ ����ϱ� ���� Ÿ�� �±׷� ������
    private Airplane hitTarget; // ���� Ÿ���� Airplane�� �ӽ÷� ĳ��
    private ObjectPool<BulletController> objectPool; // �Ѿ��� ����ϴ� �÷��̾�� ���� ObjectPool�� �����ϱ� ���� Controller���� �Ҵ����
    public void Init(string targetTag, ObjectPool<BulletController> objectPool)
    {
        this.targetTag = targetTag;
        this.objectPool = objectPool;
    }
    public void ShotBullet(Vector2 pos, Vector2 dir, AirplaneData airplaneData)
    {
        if(movementData.rigid == null)
            movementData.rigid = GetComponent<Rigidbody2D>();

        transform.position = pos; // ��ġ �ʱ�ȭ

        bulletDamage = airplaneData.attackDamage; // ���ݷ� �ʱ�ȭ
        movementData.moveDir = dir; // ���� �ʱ�ȭ

        BroadcastMessage("ObjMove", movementData); // Rigidbody2D.velocity�� ����� �̵�, Movement ������Ʈ�� ObjMove�� ��ε�ĳ����

        Invoke("DisableGO", disableTime); // �Ѿ��� ���� �� ���� ���� ��Ȱ��ȭ ��
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

        if (coll.CompareTag(targetTag)) // �浹�� ������Ʈ�� Ÿ�� �±׿� �������� �Ǵ�
        {
            hitTarget = coll.GetComponent<Airplane>();

            hitTarget.TakeDamage(bulletDamage);
            DisableGO();

            hitTarget = null;
        }
            
    }
}