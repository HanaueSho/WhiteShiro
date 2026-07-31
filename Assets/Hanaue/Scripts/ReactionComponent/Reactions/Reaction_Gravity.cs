/*
    Reaction_Gravity.cs
    20260729  hanaue sho
    落下するブロックに持たせる
*/
using UnityEngine;
using UnityEngine.EventSystems;

public class Reaction_Gravity : ReactionComponent
{
    public override void Reaction(Block influencer)
    {
        base.Reaction(influencer);

        // 重力
        Block downBlock = GetComponent<Block>().GetDownBlock();   // 足元ブロック
        Block lowerBlock = GetComponent<Block>().GetLowerBlock(); // それより下のブロック

        if (downBlock == null && lowerBlock != null )
        {
            Vector3 position = lowerBlock.transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
            transform.position = position;
        }
    }

}
