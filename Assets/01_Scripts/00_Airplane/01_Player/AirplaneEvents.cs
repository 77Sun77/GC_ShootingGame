using UnityEngine;

public class AirplaneEvents
{
    public Vector2 InputKey_Move() => Vector2.right*Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");
    public bool InputKey_Attack() => Input.GetKey(KeyCode.Space);
    public bool InputKey_AttackCharge() => Input.GetKeyUp(KeyCode.Space);
}
