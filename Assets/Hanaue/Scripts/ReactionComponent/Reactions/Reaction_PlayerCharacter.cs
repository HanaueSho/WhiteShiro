/*
    Reaction_PlayerCharacter.cs
    20260801  hanaue sho
    プレイヤー専用のリアクションを再生する
    Player, SubPlayer に持たせます。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction_PlayerCharacter : ReactionComponent
{
    // 移動後に呼んで、クリア、リセット判定を行います。

    public override bool Enter(Block influencer, CommandComponent command)
    {
        //Debug.Log("[Debug] Reaction_PlayerCharacter Enter");
        // ゴールチェック
        CheckGoal();
        // ノータッチチェック
        CheckNoTouch();

        return true;
    }

    public override IEnumerator Reaction(Block influencer)
    {
        return base.Reaction(influencer);
    }

    public override void Exit(Block influencer)
    {
        base.Exit(influencer);
    }

    // 隣接してたらアウト
    private bool CheckNoTouch()
    {
        // 周囲のブロックを検索
        List<Block> blocks = GetComponent<Block>().GetAroundBlocks();

        foreach (Block block in blocks)
        {
            if (block == null)
            {
                continue;
            }
            if (block.GetComponent<Reaction_NoTouch>() == null)
            {
                continue;
            }

            block.GetComponent<Reaction_NoTouch>()?.Enter(GetComponent<Block>(), null);
            Debug.Log("[Debug] DONT TOUCH!!!!!!!!!!!!!!!");
            return true;
        }

        return false;
    }

    private bool CheckGoal()
    {
        // 足元のブロック取得
        Block block = GetComponent<Block>().GetDownBlock(true);

        // Reaction_Goal か判定
        if (block != null)
        {
            if (block.GetComponent<Reaction_Goal>())
            {
                // Enter を呼ぶ
                block.GetComponent<Reaction_Goal>()?.Enter(GetComponent<Block>(), null);
                Debug.Log("[Debug] Get Goal!!!!!!!!!!!!!!!");
                return true;
            }
        }

        return false;
    }

}
