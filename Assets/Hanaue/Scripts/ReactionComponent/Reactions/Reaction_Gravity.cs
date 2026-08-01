/*
    Reaction_Gravity.cs
    20260729  hanaue sho
    落下するブロックに持たせる
    Enter または Exit を呼ぶと Reaction が呼び出され重力処理が始まる
*/
using System.Collections;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Reaction_Gravity : ReactionComponent
{
    private Vector3 _targetPosition = Vector3.zero;
    private Block _upBlock;

    public override bool Enter(Block influencer, CommandComponent command = null)
    {
        // 重力処理
        Block downBlock = GetComponent<Block>().GetDownBlock(true);   // 足元ブロック
        Block lowerBlock = GetComponent<Block>().GetLowerBlock(true); // それより下のブロック
        if (downBlock == null && lowerBlock != null)
        {
            _targetPosition = lowerBlock.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
            StartCoroutine(Reaction(GetComponent<Block>()));

            // 下のブロックが Reaction_IsNoTouchDestroy を持っているか判定
            bool isNoTouchDestroy = lowerBlock.GetComponent<Reaction_NoTouch_Destroy>();
            // 自分の頭の上に重力持ちがいるなら落下
            StartCoroutine(CheckUpBlockReactionGravity(isNoTouchDestroy));
        }


        return true;
    }

    public override IEnumerator Reaction(Block influencer)
    {
        base.Reaction(influencer);

        // 重力
        //Block downBlock = GetComponent<Block>().GetDownBlock();   // 足元ブロック
        //Block lowerBlock = GetComponent<Block>().GetLowerBlock(); // それより下のブロック

        //if (downBlock == null && lowerBlock != null )
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0.0f;
            float fallDuration = 0.5f;
            while (elapsedTime < fallDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fallDuration);
                transform.position = Vector3.Lerp(startPosition, _targetPosition, t);

                yield return null; // 次フレームまで待つ
            }

            transform.position = _targetPosition;
        }

        // ----- Button Enter -----
        Block downBlock = GetComponent<Block>().GetDownBlock(true);
        downBlock?.GetComponent<Reaction_Button>()?.Enter(GetComponent<Block>(), null);

        // ----- No Touch Destroy -----
        downBlock?.GetComponent<Reaction_NoTouch_Destroy>()?.Enter(GetComponent<Block>(), null);

        // ----- PlayerCharacter Enter -----
        GetComponent<Reaction_PlayerCharacter>()?.Enter(GetComponent<Block>(), null);

        yield break;
    }
    public override void Exit(Block influencer)
    {
        base.Exit(influencer);

        // 重力処理
        Enter(influencer);

    }
    
    public IEnumerator CheckUpBlockReactionGravity(bool isNoTouchDestroy = false)
    {
        Block upBlock = GetComponent<Block>().GetUpBlock(true);
        Reaction_Gravity gravity = upBlock?.GetComponent<Reaction_Gravity>();
        if (gravity)
        {
            _upBlock = upBlock;
            // 目標位置を一つ上げる
            gravity._targetPosition = _targetPosition + new Vector3(0.0f, 1.0f, 0.0f);
            if (isNoTouchDestroy)
            {
                gravity._targetPosition = _targetPosition;
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(gravity.Reaction(GetComponent<Block>()));

            // 自分の頭の上に重力持ちがいるなら落下
            StartCoroutine(gravity.CheckUpBlockReactionGravity(isNoTouchDestroy));
        }


        yield break;
    }
}
