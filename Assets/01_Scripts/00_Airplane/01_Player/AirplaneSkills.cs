using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AirplaneSkills // Airplane을 상속받는 플레이어와 적이 공격하는 방식을 모아서 구현한 것
{
    // BasicShot의 경우 매서드 오버로딩을 사용
    public void BasicShot(AirplaneController airplane) // 플레이어가 발사하는 기본 공격
    {
        airplane.airplaneData.isAttacking = true;
        airplane.Invoke("ResetAttack", airplane.airplaneData.attackSpeed);
        for (int i = 0; i < airplane.airplaneData.bulletSpawn.Length; i++) // 플레이어가 한번의 공격에 여러 총알을 발사하는 경우가 있어 스폰을 배열로 받고 그 장소에 스폰되게 처리
            BulletManager.Instance.GetPlayerBullet().ShotBullet(airplane.airplaneData.bulletSpawn[i].position, Vector2.up, airplane.airplaneData);
    }
    public void BasicShot(Enemy_Range enemy, Vector3 targetPos) // 적이 발사하는 기본 공격
    {
        enemy.airplaneData.isAttacking = true;
        enemy.Invoke("ResetAttack", enemy.airplaneData.attackSpeed);

        BulletManager.Instance.GetEnemyBullet().ShotBullet(enemy.transform.position, (targetPos - enemy.transform.position).normalized, enemy.airplaneData);

    }
    public void TripleShot(Enemy_Elite enemy, Vector3 dir) => BulletManager.Instance.GetEnemyBullet().ShotBullet(enemy.transform.position, dir.normalized, enemy.airplaneData); // 가운대를 기준으로 부채꼴로 3개 총알을 발사하는 적의 공격

}
