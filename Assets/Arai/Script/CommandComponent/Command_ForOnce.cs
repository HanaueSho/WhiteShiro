/*
    CommandForOnce.cs
    20260728  arai eito
    コマンド　1回のみのFor文
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_ForOnce", menuName = "Scriptable Objects/Command/ForOnce")]
public class Command_ForOnce : Command_While
{
    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "プログラム";
    }

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    protected override bool OnLoopEnd()
    {
        return true;
    }
}
