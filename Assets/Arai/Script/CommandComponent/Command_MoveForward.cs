/*
    CommandMoveForward.cs
    20260728  arai eito
    コマンド前方移動
*/
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Command_MoveForward", menuName = "Scriptable Objects/Command/MoveForward")]
public class Command_MoveForward : CommandComponent
{
    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "前進しろ";
    }
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
        if (_upReaction == null && upBlock?.GetComponent<Reaction_Gravity>())
        {
            _upReaction = upBlock.GetComponent<Reaction_Gravity>();
        }
    }

    public override IEnumerator Command(CommandPlayer owner, Action<bool> result)

    {
        base.Command(owner, result);

        // IsCanMove
        if (!_IsCanMove)
        {
            result(true);
            yield break; 
        }

        // ----- Follow Reaction -----
        //yield return _upReaction?.Reaction(owner.GetComponent<Block>());

        // ----- Pushed Reaction -----
        //yield return _forwardReaction?.Reaction(owner.GetComponent<Block>());

        // 正面チェック
        Block forwardBlock = owner.GetComponent<Block>().GetForwardBlock();
        if (forwardBlock != null && _forwardReaction == null)
        {
            // 正面にブロックがあるので進まない
            result(true); // コマンド自体は消費する
            yield break;
        }

        // ----- 移動処理 -----
        Vector3 startPosition = owner.transform.position;
        Vector3 targetPosition = owner.transform.position + owner.transform.forward;
        float elapsedTime = 0.0f;
        float moveDuration = 0.5f;
        while(elapsedTime < moveDuration)
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
        _forwardReaction?.Exit(owner.GetComponent<Block>());

        yield break;
    }
}
