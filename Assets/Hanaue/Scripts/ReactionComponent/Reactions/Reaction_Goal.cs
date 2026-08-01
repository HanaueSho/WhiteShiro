/*
    Reaction_Goal.cs
    20260801  hanaue sho
    ゴールのリアクションを再生する
    Goal に持たせます。
*/
using UnityEngine;

public class Reaction_Goal : ReactionComponent
{
    public override bool Enter(Block influencer, CommandComponent command)
    {
        // ここでクリアを通知
        return base.Enter(influencer, command);
    }

}
