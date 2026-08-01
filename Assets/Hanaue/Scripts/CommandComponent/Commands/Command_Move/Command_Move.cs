/*
    Command_Move.cs
    20260731  hanaue sho
    コマンドによる移動の基底クラス
*/
using System;
using System.Collections;
using UnityEngine;

public class Command_Move : CommandComponent
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    protected ReactionComponent _moveDirectionReaction; // 進む方向のリアクション参照
    protected ReactionComponent _upReaction; // 上部のリアクション参照
    // 動けるかフラグ
    protected bool _isCanMove = true;
    // 動く方向
    protected Vector3 _moveDirection = Vector3.zero;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public Vector3 MoveDirection => _moveDirection;


    // ==================================================
    // ----- Lifecycle -----
    // ==================================================
    public override void Enter(CommandPlayer owner)
    {
        // 初期化
        _moveDirectionReaction = null;
        _upReaction = null;
        _isCanMove = true;

        // ----- 正面チェック -----
        Block moveDirectionBlock = owner.GetComponent<Block>().GetBlock(_moveDirection, true);
        if (moveDirectionBlock != null)
        {
            // 正面にブロックがあるので動けない
            _isCanMove = false;
        }

        // ----- Pushed Enter -----
        _moveDirectionReaction = moveDirectionBlock?.GetComponent<Reaction_Pushed>();
        _isCanMove = _moveDirectionReaction?.Enter(owner.GetComponent<Block>(), this) ?? _isCanMove; // 動けるか判定

        // ----- Follow Enter -----
        Block upBlock = owner.GetComponent<Block>().GetUpBlock(true);
        _upReaction = upBlock?.GetComponent<Reaction_Follow>();
        _upReaction?.Enter(owner.GetComponent<Block>(), this);
        if (_upReaction == null && upBlock?.GetComponent<Reaction_Gravity>())
        {
            _upReaction = upBlock.GetComponent<Reaction_Gravity>();
        }

        // ----- Button Exit -----
        Block downBlock = owner.GetComponent<Block>().GetDownBlock(true);
        downBlock?.GetComponent<Reaction_Button>()?.Exit(owner.GetComponent<Block>());

    }

    public override IEnumerator Command(CommandPlayer owner, Action<bool> result)
    {
        base.Command(owner, result);

        // IsCanMove
        if (!_isCanMove)
        {
            result(true);
            yield break;
        }

        // ----- Follow Reaction -----
        //yield return _upReaction?.Reaction(owner.GetComponent<Block>());
        // ----- Pushed Reaction -----
        //yield return _forwardReaction?.Reaction(owner.GetComponent<Block>());

        // ----- 移動処理 -----
        Vector3 startPosition = owner.transform.position;
        Vector3 targetPosition = owner.transform.position + _moveDirection;
        float elapsedTime = 0.0f;
        float moveDuration = 0.5f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            owner.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null; // 次フレームまで待つ
        }
        owner.transform.position = targetPosition;

        result(true);
        yield break;
    }

    public override IEnumerator Exit(CommandPlayer owner)
    {
        // 重力適用
        if (owner.GetComponent<Reaction_Gravity>() && owner.GetComponent<Block>())
        {
            owner.GetComponent<Reaction_Gravity>().Enter(owner.GetComponent<Block>());
        }

        // ----- Follow Exit -----
        _upReaction?.Exit(owner.GetComponent<Block>());
        // ----- Pushed Exit -----
        _moveDirectionReaction?.Exit(owner.GetComponent<Block>());

        // ----- Button Enter -----
        Block downBlock = owner.GetComponent<Block>().GetDownBlock(true);
        downBlock?.GetComponent<Reaction_Button>()?.Enter(owner.GetComponent<Block>(), this);

        // 初期化
        _moveDirection = Vector3.zero;

        yield break;
    }


    // ==================================================
    // ----- Setter -----
    // ==================================================
    public void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction.normalized;
    }
}
