/*
    CommandMoveForward.cs
    20260728  arai eito
    コマンド前方移動
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_MoveForward", menuName = "Scriptable Objects/Command/MoveForward")]
public class Command_MoveForward : CommandComponent
{
    // 動けるかフラグ
    private bool _IsCanMove = true;


    public override void Enter(CommandPlayer owner)
    {
        // 初期化
        _forwardReaction = null;
        _upReaction = null;

        // ----- Pushed Enter -----
        Block forwardBlock = owner.GetComponent<Block>().GetForwardBlock();
        _forwardReaction = forwardBlock?.GetComponent<Reaction_Pushed>();
        _IsCanMove = _forwardReaction?.Enter(owner.GetComponent<Block>()) ?? true; // 動けるか判定
        
        // ----- Follow Enter -----
        Block upBlock = owner.GetComponent<Block>().GetUpBlock();
        _upReaction = upBlock?.GetComponent<Reaction_Follow>();
        _upReaction?.Enter(owner.GetComponent<Block>());
    }

    public override bool Command(CommandPlayer owner)
    {
        base.Command(owner);

        // IsCanMove
        if (!_IsCanMove)
        {
            return true;
        }

        // ----- Follow Reaction -----
        _upReaction?.Reaction(owner.GetComponent<Block>());

        // ----- Pushed Reaction -----
        _forwardReaction?.Reaction(owner.GetComponent<Block>());

        // 正面チェック
        Block forwardBlock = owner.GetComponent<Block>().GetForwardBlock();
        if (forwardBlock != null && _forwardReaction == null)
        {
            // 正面にブロックがあるので進まない
            return true; // コマンド自体は消費する
        }
        // 移動処理
        Transform block = owner?.transform;
        if(block != null )
        {
            block.position += block.forward;
        }

        return true;
    }

    public override void Exit(CommandPlayer owner)
    {
        // 重力適用
        if (owner.GetComponent<Reaction_Gravity>() && owner.GetComponent<Block>())
        {
            owner.GetComponent<Reaction_Gravity>().Reaction(owner.GetComponent<Block>());
        }

        // ----- Follow Exit -----
        _upReaction?.Exit(owner.GetComponent<Block>());
        // ----- Pushed Exit -----
        _forwardReaction?.Exit(owner.GetComponent<Block>());

    }
}
