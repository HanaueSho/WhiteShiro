/*
    Reaction_NoTouch.cs
    20260801  hanaue sho
    触れたら終わりのリアクションを再生する
*/
using UnityEngine;

public class Reaction_NoTouch : ReactionComponent
{
    public override bool Enter(Block influencer, CommandComponent command)
    {
        // ここで死亡を通知
        return base.Enter(influencer, command);
    }
}
