using Unity.VisualScripting;
using UnityEngine;

public class AirplaneController : Airplane
{
    private AirplaneEvents airplaneEvents = new(); // 이벤트 구현
    private AirplaneSkills airplaneSkills = new(); // 스킬 구현

    [SerializeField] private MovementData movementData; // Movement 클래스를 사용하기 위한 데이터 구조체
    private float min_X = -2.75f; // 맵 경계 최솟값
    private float max_X = 2.75f; // 맵 경계 최댓값
    private float cur_X; // 현재 X 위치

    public HPBar hpBar; // 체력 UI
    private void Start()
    {
        movementData.rigid = GetComponent<Rigidbody2D>();
        BulletManager.Instance.CreatePlayerBullet(airplaneData.bulletPrefab, airplaneData.bulletCount, "Enemy"); // 오브젝트 풀로 초기 총알 생성
        hpBar.gameObject.SetActive(true);
    }
    private void Update()
    {
        AirplaneMove(); // 이동
        AirplaneAttack(); // 공격
        SetHPBarPosition(); // 체력바 위치
    }
    
    public override void AirplaneMove()
    {
        if(movementData.rigid != null)
        {
            movementData.moveDir = airplaneEvents.InputKey_Move().normalized; // 움직임 키 이벤트를 감지했다면 moveDir에 값이 들어온다
            BroadcastMessage("ObjMove", movementData); // Movement 컴포넌트의 ObjMove를 브로드캐스팅
            cur_X = Mathf.Clamp(transform.position.x, min_X, max_X); // 위치 제한
            transform.position = (Vector3.right * cur_X) + (Vector3.up * transform.position.y);
        }
    }
    protected override void AirplaneAttack()
    {
        if (airplaneEvents.InputKey_Attack() && !airplaneData.isAttacking) // 공격 키 이벤트를 감지했다면 True를 반환한다
        {
            airplaneSkills.BasicShot(this); // 공격
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
