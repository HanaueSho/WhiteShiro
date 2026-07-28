/*
    CommandComponent.cs
    20260728  hanaue sho
    コマンドの基底クラス
    CommandPlayer から順に呼ばれる
*/
using UnityEngine;

public class CommandComponent : ScriptableObject
{
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
