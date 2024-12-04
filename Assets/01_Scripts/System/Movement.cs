using UnityEngine;

public class Movement : MonoBehaviour
{
    public void ObjMove(MovementData movementData) // ��ε�ĳ��Ʈ �Ż����� ���� ����
    {
        if(gameObject.activeInHierarchy && movementData.rigid)
        {
            movementData.rigid.linearVelocity = movementData.moveDir * movementData.moveSpeed;
        }
    }
}
[System.Serializable]
public struct MovementData // �̵��� ���� �����͸� ����
{
    public Rigidbody2D rigid; // �̵��� ������Ʈ�� Rigidbody2D
    public float moveSpeed; // �̵� �ӵ�
    public Vector2 moveDir; // �̵� ����
}