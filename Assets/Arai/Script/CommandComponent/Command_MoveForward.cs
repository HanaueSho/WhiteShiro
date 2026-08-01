/*
    CommandMoveForward.cs
    20260728  arai eito
    コマンド前方移動
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_MoveForward", menuName = "Scriptable Objects/Command/MoveForward")]
public class Command_MoveForward : Command_Move
{
    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "前進しろ";
    }


    // ==================================================
    // ----- Enter -----
    // ==================================================
    public override void Enter(CommandPlayer owner)
    {
        // 進行方向を設定
        _moveDirection = owner.transform.TransformDirection(Vector3.forward);

        // base Enter
        base.Enter(owner);
    }
}
