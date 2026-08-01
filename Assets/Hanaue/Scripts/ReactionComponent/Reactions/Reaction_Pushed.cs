/*
    Reaction_Pushed.cs
    20260730  hanaue sho
    押せるブロックに持たせる
*/
using System.Collections;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Reaction_Pushed : ReactionComponent
{
    private Vector3 _moveDirection;
    private Reaction_Follow _upReactionFollow; // 上部のリアクション参照
    private Reaction_Pushed _forwardReactionPushed; // 正面のリアクション参照

    // true: 動く, false: 動かない
    public override bool Enter(Block influencer, CommandComponent command)
    {
        // 初期化
        _forwardReactionPushed = null;

        // influencer の正面方向へ移動する
        _moveDirection = ((Command_Move)command).MoveDirection;

        bool result = true;
        // ブロックチェック
        Block block = null;
        if (Physics.Raycast(transform.position, _moveDirection, out RaycastHit hit, 1.0f))
        {
            if (hit.transform.GetComponent<Block>())
            {
                // 移動方向にブロックがあるので動かない
                block = hit.transform.GetComponent<Block>();
                result = false;
            }
            if (hit.transform.GetComponent<Reaction_Pushed>())
            {
                // 移動方向が Pushed なら呼ぶ
                _forwardReactionPushed = hit.transform.GetComponent<Reaction_Pushed>();
                result = _forwardReactionPushed.Enter(influencer, command);
            }
        }
        if (result)
        {
            transform.SetParent(influencer.transform, true);
        }

        // ----- 上部確認 ------
        _upReactionFollow = null; // 初期化
        Block upBlock = GetComponent<Block>().GetUpBlock(true);
        _upReactionFollow = upBlock?.GetComponent<Reaction_Follow>();
        _upReactionFollow?.Enter(influencer, command);

        // ----- Button Exit -----
        Block downBlock = GetComponent<Block>().GetDownBlock(true);
        downBlock?.GetComponent<Reaction_Button>()?.Exit(GetComponent<Block>());

        return result;
    }

    public override IEnumerator Reaction(Block influencer)
    {
        base.Reaction(influencer);

        // ----- 上部リアクション -----
        yield return _upReactionFollow?.Reaction(influencer);
        yield return _forwardReactionPushed?.Reaction(influencer);

        yield break;
    }

    public override void Exit(Block influencer)
    {
        base.Exit(influencer);

        // 親子関係設定を切る
        transform.SetParent(null, true);

        // 重力処理
        if (GetComponent<Reaction_Gravity>() is Reaction_Gravity gravity)
        {
            gravity.Enter(GetComponent<Block>());
        }

        // ----- 上部リアクション -----
        _upReactionFollow?.Exit(influencer);
        _forwardReactionPushed?.Exit(influencer);

        // ----- Button Enter -----
        Block downBlock = GetComponent<Block>().GetDownBlock(true);
        downBlock?.GetComponent<Reaction_Button>()?.Enter(GetComponent<Block>(), null);
    }
}
