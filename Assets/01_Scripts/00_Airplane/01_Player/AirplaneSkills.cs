using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AirplaneSkills
{
    public void BasicShot(AirplaneController airplane)
    {
        airplane.airplaneData.isAttacking = true;
        airplane.Invoke("ResetAttack", airplane.airplaneData.attackSpeed);
        for (int i = 0; i < airplane.airplaneData.bulletSpawn.Length; i++)
            BulletManager.Instance.GetPlayerBullet().ShotBullet(airplane.airplaneData.bulletSpawn[i].position, Vector2.up, airplane.airplaneData);
    }
    public void BasicShot(Enemy_Range enemy, Vector3 targetPos)
    {
        enemy.airplaneData.isAttacking = true;
        enemy.Invoke("ResetAttack", enemy.airplaneData.attackSpeed);

        BulletManager.Instance.GetEnemyBullet().ShotBullet(enemy.transform.position, (targetPos - enemy.transform.position).normalized, enemy.airplaneData);

    }
    public void TripleShot(Enemy_Elite enemy, Vector3 dir) => BulletManager.Instance.GetEnemyBullet().ShotBullet(enemy.transform.position, dir.normalized, enemy.airplaneData);

}
