using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AirplaneSkills // Airplane�� ��ӹ޴� �÷��̾�� ���� �����ϴ� ����� ��Ƽ� ������ ��
{
    // BasicShot�� ��� �ż��� �����ε��� ���
    public void BasicShot(AirplaneController airplane) // �÷��̾ �߻��ϴ� �⺻ ����
    {
        airplane.airplaneData.isAttacking = true;
        airplane.Invoke("ResetAttack", airplane.airplaneData.attackSpeed);
        for (int i = 0; i < airplane.airplaneData.bulletSpawn.Length; i++) // �÷��̾ �ѹ��� ���ݿ� ���� �Ѿ��� �߻��ϴ� ��찡 �־� ������ �迭�� �ް� �� ��ҿ� �����ǰ� ó��
            BulletManager.Instance.GetPlayerBullet().ShotBullet(airplane.airplaneData.bulletSpawn[i].position, Vector2.up, airplane.airplaneData);
    }
    public void BasicShot(Enemy_Range enemy, Vector3 targetPos) // ���� �߻��ϴ� �⺻ ����
    {
        enemy.airplaneData.isAttacking = true;
        enemy.Invoke("ResetAttack", enemy.airplaneData.attackSpeed);

        BulletManager.Instance.GetEnemyBullet().ShotBullet(enemy.transform.position, (targetPos - enemy.transform.position).normalized, enemy.airplaneData);

    }
    public void TripleShot(Enemy_Elite enemy, Vector3 dir) => BulletManager.Instance.GetEnemyBullet().ShotBullet(enemy.transform.position, dir.normalized, enemy.airplaneData); // ����븦 �������� ��ä�÷� 3�� �Ѿ��� �߻��ϴ� ���� ����

}
