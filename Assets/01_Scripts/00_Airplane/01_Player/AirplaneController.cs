using Unity.VisualScripting;
using UnityEngine;

public class AirplaneController : Airplane
{
    private AirplaneEvents airplaneEvents = new(); // �̺�Ʈ ����
    private AirplaneSkills airplaneSkills = new(); // ��ų ����

    [SerializeField] private MovementData movementData; // Movement Ŭ������ ����ϱ� ���� ������ ����ü
    private float min_X = -2.75f; // �� ��� �ּڰ�
    private float max_X = 2.75f; // �� ��� �ִ�
    private float cur_X; // ���� X ��ġ

    public HPBar hpBar; // ü�� UI
    private void Start()
    {
        movementData.rigid = GetComponent<Rigidbody2D>();
        BulletManager.Instance.CreatePlayerBullet(airplaneData.bulletPrefab, airplaneData.bulletCount, "Enemy"); // ������Ʈ Ǯ�� �ʱ� �Ѿ� ����
        hpBar.gameObject.SetActive(true);
    }
    private void Update()
    {
        AirplaneMove(); // �̵�
        AirplaneAttack(); // ����
        SetHPBarPosition(); // ü�¹� ��ġ
    }
    
    public override void AirplaneMove()
    {
        if(movementData.rigid != null)
        {
            movementData.moveDir = airplaneEvents.InputKey_Move().normalized; // ������ Ű �̺�Ʈ�� �����ߴٸ� moveDir�� ���� ���´�
            BroadcastMessage("ObjMove", movementData); // Movement ������Ʈ�� ObjMove�� ��ε�ĳ����
            cur_X = Mathf.Clamp(transform.position.x, min_X, max_X); // ��ġ ����
            transform.position = (Vector3.right * cur_X) + (Vector3.up * transform.position.y);
        }
    }
    protected override void AirplaneAttack()
    {
        if (airplaneEvents.InputKey_Attack() && !airplaneData.isAttacking) // ���� Ű �̺�Ʈ�� �����ߴٸ� True�� ��ȯ�Ѵ�
        {
            airplaneSkills.BasicShot(this); // ����
        }

    }
    public override void Heal()
    {
        base.Heal();
        hpBar.UpdateHPBar(airplaneData);
    }
    public override void TakeDamage(float takeAttackDamage)
    {
        base.TakeDamage(takeAttackDamage);
        hpBar.UpdateHPBar(airplaneData);
    }
    private void SetHPBarPosition()
    {
        hpBar.transform.position = transform.position + (Vector3.up*0.5f);
    }
    protected override void Die()
    {
        GameManager.Instance.EndStage(false);
    }
}
