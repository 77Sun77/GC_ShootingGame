using UnityEngine;

public class Movement : MonoBehaviour
{
    public void ObjMove(MovementData movementData)
    {
        if(gameObject.activeInHierarchy && movementData.rigid)
        {
            movementData.rigid.linearVelocity = movementData.moveDir * movementData.moveSpeed;
        }
    }
}
[System.Serializable]
public struct MovementData
{
    public Rigidbody2D rigid;
    public float moveSpeed;
    public Vector2 moveDir;
}