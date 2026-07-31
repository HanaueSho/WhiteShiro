/*
    Command_Move_Right.cs
    20260731  hanaue sho
    コマンドの右移動
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_Move_Right", menuName = "Scriptable Objects/Command/Move_Right")]
public class Command_Move_Right : Command_Move
{
    // ==================================================
    // ----- Enter -----
    // ==================================================
    public override void Enter(CommandPlayer owner)
    {
        // 進行方向を設定
        _moveDirection = owner.transform.TransformDirection(Vector3.right);

        // base Enter
        base.Enter(owner);
    }
}
