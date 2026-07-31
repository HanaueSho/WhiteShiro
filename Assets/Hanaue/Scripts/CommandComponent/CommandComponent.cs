/*
    CommandComponent.cs
    20260728  hanaue sho
    コマンドの基底クラス
    CommandPlayer から順に呼ばれる
*/
using System;
using System.Collections;
using UnityEngine;

public class CommandComponent : ScriptableObject
{

    // ==================================================
    // ----- Propaty -----
    // ==================================================
    protected string _visualText;
    protected ReactionComponent _forwardReaction; // 前方のリアクション参照
    protected ReactionComponent _upReaction; // 上部のリアクション参照

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public string VisualText => _visualText;


    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "命令しろ";
    }

    // ==================================================
    // ----- Public Event -----
    // ==================================================

    public virtual void Initialize()
    {
        // コマンドの初期化
    }

    public virtual void Enter(CommandPlayer owner)
    {
        // リアクションを呼ぶ
    }

    public virtual IEnumerator Command(CommandPlayer owner, Action<bool> b)
    {
        Debug.Log($"[Command] {GetType()}");
        if (b != null)
        {
            b(true);
        }
        yield break;
    }

    public virtual IEnumerator Exit(CommandPlayer owner)
    {
        // リアクションを呼ぶ
        yield break;
    }
}
