/*
    Reaction_Piston.cs
    20260801  hanaue sho
    ピストンコマンド
    指定方向に動く
*/
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Command_Piston", menuName = "Scriptable Objects/Command/Piston")]
public class Command_Piston : Command_Move
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    // 動く方向
    public enum Direction
    {
        Forward, Back, Up, Down, Right, Left
    }
    [SerializeField] private Direction _direction = Direction.Forward;
    public Vector3 PistonMoveDirection => _direction switch
    {
        Direction.Forward => Vector3.forward,
        Direction.Back    => Vector3.back,
        Direction.Up      => Vector3.up,
        Direction.Down    => Vector3.down,
        Direction.Right   => Vector3.right,
        Direction.Left    => Vector3.left,
        _                 => Vector3.zero
    };
    // 反転移動
    [SerializeField] private bool _isReturn = false;

    // ==================================================
    // ----- Lifecycle -----
    // ==================================================
    public override void Initialize()
    {
        _isReturn = false;
    }

    public override void Enter(CommandPlayer owner)
    {
        _moveDirection = PistonMoveDirection;
        if (_isReturn)
        {
            _moveDirection = -_moveDirection;
        }
        //Debug.Log("[Debug] Command_Piston");
        base.Enter(owner);

        // 動けるなら切り替える
        if (_isCanMove)
        {
            // フラグ切り替え
            _isReturn = !_isReturn;
        }
    }
}
