using Unity.VisualScripting;
using UnityEngine;

public class AirplaneController : Airplane
{
    private AirplaneEvents airplaneEvents = new();
    private AirplaneSkills airplaneSkills = new();
    [SerializeField] private MovementData movementData;
    public BulletController skillLazer;
    public bool isUseSkill;
    private float min_X = -2.75f;
    private float max_X = 2.75f;
    private float cur_X;

    public HPBar hpBar;
    private void Start()
    {
        movementData.rigid = GetComponent<Rigidbody2D>();
        BulletManager.Instance.CreatePlayerBullet(airplaneData.bulletPrefab, airplaneData.bulletCount, "Enemy");
        hpBar.gameObject.SetActive(true);
    }
    private void Update()
    {
        AirplaneMove();
        AirplaneAttack();
        SetHPBarPosition();
    }
    
    public override void AirplaneMove()
    {
        if(movementData.rigid != null)
        {
            movementData.moveDir = airplaneEvents.InputKey_Move().normalized;
            BroadcastMessage("ObjMove", movementData);
            cur_X = Mathf.Clamp(transform.position.x, min_X, max_X);
            transform.position = (Vector3.right * cur_X) + (Vector3.up * transform.position.y);
        }
    }
    protected override void AirplaneAttack()
    {
        if (airplaneEvents.InputKey_Attack() && !airplaneData.isAttacking && !isUseSkill)
        {
            airplaneSkills.BasicShot(this);
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
    private void ResetSkill() => isUseSkill = false;
    protected override void Die()
    {
        GameManager.Instance.EndStage(false);
    }
}
