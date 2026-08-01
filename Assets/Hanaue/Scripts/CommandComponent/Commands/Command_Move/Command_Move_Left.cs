/*
    Command_Move_Left.cs
    20260731  hanaue sho
    コマンドの左移動
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_Move_Left", menuName = "Scriptable Objects/Command/Move_Left")]
public class Command_Move_Left : Command_Move
{
    // ==================================================
    // ----- Enter -----
    // ==================================================
    public override void Enter(CommandPlayer owner)
    {
        // 進行方向を設定
        _moveDirection = owner.transform.TransformDirection(Vector3.left);

        // base Enter
        base.Enter(owner);
    }
}
