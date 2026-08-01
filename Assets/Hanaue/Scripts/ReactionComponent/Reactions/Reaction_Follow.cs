/*
    Reaction_Follow.cs
    20260730  hanaue sho
    動くものに追従するブロックに持たせる
*/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

public class Reaction_Follow : ReactionComponent
{
    private Reaction_Follow _upReactionFollow; // 上部のリアクション参照
    private Transform _originParent;

    private void Start()
    {
        _originParent = transform.parent;
    }


    public override bool Enter(Block influencer, CommandComponent command)
    {
        // ----- 親子関係設定 -----
        // influencer の正面方向をチェック
        Vector3 moveDirection = ((Command_Move)command)?.MoveDirection ?? Vector3.zero;

        // ブロックチェック
        Block moveDirectionBlock = GetComponent<Block>().GetBlock(moveDirection, true);
        if (moveDirectionBlock?.GetComponent<Reaction_Pushed>()?.Enter(influencer, command) ?? false)
        {
            // 押して動けるので null
            moveDirectionBlock = null;
        }

        if (moveDirectionBlock == null)
        {
            transform.SetParent(influencer.transform, true);

            // ----- 上部 Follow ------
            _upReactionFollow = null; // 初期化
            Block upBlock = GetComponent<Block>().GetUpBlock(true);
            _upReactionFollow = upBlock?.GetComponent<Reaction_Follow>();
            _upReactionFollow?.Enter(influencer, command);

            // ----- Button Exit -----
            Block downBlock = GetComponent<Block>().GetDownBlock(true);
            downBlock?.GetComponent<Reaction_Button>()?.Exit(GetComponent<Block>());
        }

        return true;
    }

    public override IEnumerator Reaction(Block influencer)
    {
        base.Reaction(influencer);

        // ----- 上部リアクション -----
        _upReactionFollow?.Reaction(influencer);

        yield break;
    }

    public override void Exit(Block influencer)
    {
        base.Exit(influencer);

        // 親子関係設定を切る
        transform.SetParent(_originParent, true);

        // 重力処理
        if (GetComponent<Reaction_Gravity>() is Reaction_Gravity gravity)
        {
            gravity.Enter(GetComponent<Block>());
        }

        // ----- 上部リアクション -----
        _upReactionFollow?.Exit(influencer);

        // ----- Button Enter -----
        Block downBlock = GetComponent<Block>().GetDownBlock(true);
        downBlock?.GetComponent<Reaction_Button>()?.Enter(GetComponent<Block>(), null);
    }

}
