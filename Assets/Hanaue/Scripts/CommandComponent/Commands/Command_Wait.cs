/*
    Command_Wait.cs
    20260802  hanaue sho
    コマンドによる待機
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_Wait", menuName = "Scriptable Objects/Command/Wait")]
public class Command_Wait : CommandComponent
{
    private void OnEnable()
    {
        _visualText = "待機しろ";
    }

    // 何もない
    // アニメーションを再生とかしたいならEnterとかでね。
}
