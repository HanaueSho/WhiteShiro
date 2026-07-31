/*
    CommandComponent.cs
    20260728  hanaue sho
    コマンドの基底クラス
    CommandPlayer から順に呼ばれる
*/
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

    public virtual bool Command(CommandPlayer owner)
    {
        Debug.Log($"[Command] {GetType()}");
        return true;
    }

    public virtual void Exit(CommandPlayer owner)
    {
        // リアクションを呼ぶ
    }
}
