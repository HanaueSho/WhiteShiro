/*
    Reaction_Follow.cs
    20260730  hanaue sho
    動くものに追従するブロックに持たせる
*/
using UnityEngine;

public class Reaction_Follow : ReactionComponent
{
    private Reaction_Follow _upReactionFollow; // 上部のリアクション参照

    public override bool Enter(Block influencer)
    {
        base.Enter(influencer);

        // ----- 親子関係設定 -----
        // influencer の正面方向をチェック
        Vector3 moveDirection = influencer.transform.forward;
        // ブロックチェック
        Block block = null;
        if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, 1.0f))
        {
            if (hit.transform.GetComponent<Block>())
            {
                block = hit.transform.GetComponent<Block>();
            }
            if (hit.transform.GetComponent<Reaction_Pushed>())
            {
                // 移動方向が Pushed なら平気
                if (hit.transform.GetComponent<Reaction_Pushed>().Enter(influencer))
                {
                    block = null;
                }
            }
        }
        if (block == null)
        {
            transform.SetParent(influencer.transform, true);

            // ----- 上部 Follow ------
            _upReactionFollow = null; // 初期化
            Block upBlock = GetComponent<Block>().GetUpBlock();
            _upReactionFollow = upBlock?.GetComponent<Reaction_Follow>();
            _upReactionFollow?.Enter(influencer);
        }

        return true;
    }

    public override void Reaction(Block influencer)
    {
        base.Reaction(influencer);

        // ----- 上部リアクション -----
        _upReactionFollow?.Reaction(influencer);
    }

    public override void Exit(Block influencer)
    {
        base.Exit(influencer);

        // 親子関係設定を切る
        transform.SetParent(null, true);

        // 重力処理
        if (GetComponent<Reaction_Gravity>() is Reaction_Gravity gravity)
        {
            gravity.Reaction(GetComponent<Block>());
        }

        // ----- 上部リアクション -----
        _upReactionFollow?.Exit(influencer);

    }

}
