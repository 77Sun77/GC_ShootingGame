using UnityEngine;

public class Movement : MonoBehaviour
{
    public void ObjMove(MovementData movementData) // 브로드캐스트 매새지를 통해 실행
    {
        if(gameObject.activeInHierarchy && movementData.rigid)
        {
            movementData.rigid.linearVelocity = movementData.moveDir * movementData.moveSpeed;
        }
    }
}
[System.Serializable]
public struct MovementData // 이동에 관한 데이터를 저장
{
    public Rigidbody2D rigid; // 이동할 오브젝트의 Rigidbody2D
    public float moveSpeed; // 이동 속도
    public Vector2 moveDir; // 이동 방향
}