/*
    Command_Move_Back.cs
    20260731  hanaue sho
    コマンドの後退移動
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_Move_Back", menuName = "Scriptable Objects/Command/Move_Back")]
public class Command_Move_Back : Command_Move
{
    // ==================================================
    // ----- Enter -----
    // ==================================================
    public override void Enter(CommandPlayer owner)
    {
        // 進行方向を設定
        _moveDirection = owner.transform.TransformDirection(Vector3.back);

        // base Enter
        base.Enter(owner);
    }
}
