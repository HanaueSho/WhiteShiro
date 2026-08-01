/*
    Command_Turn.cs
    20260731  hanaue sho
    コマンドによる旋回の基底クラス
*/
using System;
using System.Collections;
using UnityEngine;

public class Command_Turn : CommandComponent
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    protected ReactionComponent _upReaction; // 上部のリアクション参照
    protected float _turnAngle = 0.0f; // 回転角度


    // ==================================================
    // ----- Lifecycle -----
    // ==================================================
    public override void Enter(CommandPlayer owner)
    {
        // 初期化
        _upReaction = null;

        // ----- Follow Enter -----
        Block upBlock = owner.GetComponent<Block>().GetUpBlock(true);
        _upReaction = upBlock?.GetComponent<Reaction_Follow>();
        _upReaction?.Enter(owner.GetComponent<Block>(), null);
    }

    public override IEnumerator Command(CommandPlayer owner, Action<bool> result)
    {
        base.Command(owner, result);

        // ----- 回転処理 -----
        Quaternion startRotation = owner.transform.rotation;
        Quaternion targetRotation = owner.transform.rotation * Quaternion.Euler(new Vector3(0.0f, _turnAngle, 0.0f));
        float elapsedTime = 0.0f;
        float moveDuration = 0.5f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            owner.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // 次フレームまで待つ
        }
        owner.transform.rotation = targetRotation;

        result(true);
        yield break;
    }

    public override IEnumerator Exit(CommandPlayer owner)
    {
        // ----- Follow Exit -----
        _upReaction?.Exit(owner.GetComponent<Block>());
        
        yield break;
    }

}
