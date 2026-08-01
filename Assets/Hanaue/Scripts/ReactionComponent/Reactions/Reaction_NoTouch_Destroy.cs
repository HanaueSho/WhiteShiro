/*
    Reaction_NoTouch_Destroy.cs
    20260801  hanaue sho
    プレイヤー以外が触れたら一時的に無効化するリアクションを再生する
    Reaction_Gravity の Enter で判断しています
*/
using UnityEngine;

public class Reaction_NoTouch_Destroy : Reaction_NoTouch
{
    public override bool Enter(Block influencer, CommandComponent command)
    {
        // ここで Reaction_PlayerCharacter を持っているか判定
        if (influencer.GetComponent<Reaction_PlayerCharacter>() != null)
        {
            // プレイヤーキャラクターなので親のEnterを呼ぶ
            return base.Enter(influencer, command);
        }
        else
        {
            // プレイヤーキャラクター以外なので一時的に座標を飛ばす
            RemoveBlock(influencer);
            return true;
        }
    }

    private void RemoveBlock(Block influencer)
    {
        // 座標を -5 する
        Vector3 position = influencer.transform.position;
        position.y -= 5;
        influencer.transform.position = position;
    }

}
