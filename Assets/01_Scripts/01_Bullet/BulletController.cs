using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public MovementData movementData;
    [SerializeField] public float bulletDamage;
    public float disableTime;
    public string targetTag;
    private Airplane hitTarget;
    private ObjectPool<BulletController> objectPool;
    public void Init(string targetTag, ObjectPool<BulletController> objectPool)
    {
        this.targetTag = targetTag;
        this.objectPool = objectPool;
    }
    public void ShotBullet(Vector2 pos, Vector2 dir, AirplaneData airplaneData)
    {
        if(movementData.rigid == null)
            movementData.rigid = GetComponent<Rigidbody2D>();

        transform.position = pos;

        bulletDamage = airplaneData.attackDamage;
        movementData.moveDir = dir;

        BroadcastMessage("ObjMove", movementData);

        Invoke("DisableGO", disableTime);
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

        if (coll.CompareTag(targetTag))
        {
            if (gameObject.CompareTag("Enemy"))
                hitTarget = EnemyManager.Instance.targetAirplane;
            else
                hitTarget = coll.GetComponent<Airplane>();

            hitTarget.TakeDamage(bulletDamage);
            DisableGO();

            hitTarget = null;
        }
            
    }
}