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

    public virtual void Enter()
    {
        // リアクションを呼ぶ
    }

    public virtual bool Command(CommandPlayer owner)
    {
        Debug.Log($"{GetType()}");
        return true;
    }

    public virtual void Exit()
    {
        // リアクションを呼ぶ
    }
}
